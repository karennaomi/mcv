using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Security.Cryptography.Xml;
using System.IO;
using System.Security.Cryptography;

namespace MCV.Domain
{
	public class CertificadoDigital
	{
		public static X509Certificate2 Selecionar()
		{
			return CertificadoDigital.SelecionarPorNome("");
		}

        /// <summary>
        /// Classe responsável por fazer a busca dos certificados disponíveis na máquina.
        /// </summary>
        /// <param name="nome">Pode ser informado o nome do certificado ou não.</param>
        /// <returns>Retorna um objeto do tipo X509Certificate2.</returns>
		public static X509Certificate2 SelecionarPorNome(string nome)
		{
			X509Certificate2 _X509Cert = new X509Certificate2();

			try
			{
				X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
				store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);

				X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
				X509Certificate2Collection collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
				X509Certificate2Collection collection2 = (X509Certificate2Collection)collection.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, false);

				if(string.IsNullOrEmpty(nome))
				{
					X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(collection2, "Certificado(s) Digital(is) disponível(is)", "Selecione o Certificado Digital para uso no aplicativo", X509SelectionFlag.SingleSelection);

					if(scollection.Count == 0)
					{
						_X509Cert.Reset();
						throw (new Exception("Nenhum certificado escolhido"));
					}
					else
						_X509Cert = scollection[0];
				}
				else
				{
					X509Certificate2Collection scollection = (X509Certificate2Collection)collection2.Find(X509FindType.FindBySubjectDistinguishedName, nome, false);
					if(scollection.Count == 0)
						throw (new Exception("Nenhum certificado válido foi encontrado com o nome informado: " + nome));
					else
						_X509Cert = scollection[0];
				}

				store.Close();
				return _X509Cert;
			}
			catch(System.Exception ex)
			{
                //TODO : Retornar mensagem de erro MessageBox.Show(ex.Message, "Informação", MessageBoxButtons.OK, MessageBoxIcon.Error);
				//throw new CertificadoDigitalException(ex.Message);
			}

            return null;
		}

        public static string AssinarXmlPorElemento(string conteudoXml, X509Certificate2 certificado, string tag, bool assinarTagRps)
        {
            bool tagId = false;
            XmlDocument doc = StringToXmlDocument(conteudoXml);

            XmlNodeList listaTags = doc.GetElementsByTagName(tag);
            SignedXml signedXml;

            foreach (XmlElement infNfse in listaTags)
            {
                string id = "";
                if (infNfse.HasAttribute("id"))
                {
                    id = infNfse.Attributes.GetNamedItem("id").Value;
                }
                else if (infNfse.HasAttribute("Id"))
                {
                    id = infNfse.Attributes.GetNamedItem("Id").Value;
                    tagId = true;
                }
                else if (infNfse.HasAttribute("ID"))
                {
                    id = infNfse.Attributes.GetNamedItem("ID").Value;
                }
                else if (infNfse.HasAttribute("iD"))
                {
                    id = infNfse.Attributes.GetNamedItem("iD").Value;
                }
                else
                {
                    tagId = false;

                    if (assinarTagRps)
                    {
                        continue;
                    }
                }

                signedXml = new SignedXml(infNfse);
                signedXml.SigningKey = certificado.PrivateKey;

                Reference reference = new Reference("#" + id);
                if (!tagId) { reference.Uri = string.Empty; }

                reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
                reference.AddTransform(new XmlDsigC14NTransform());

                signedXml.AddReference(reference);

                KeyInfo keyInfo = new KeyInfo();
                keyInfo.AddClause(new KeyInfoX509Data(certificado));
                signedXml.KeyInfo = keyInfo;

                signedXml.ComputeSignature();

                XmlElement xmlSignature = doc.CreateElement("Signature", "http://www.w3.org/2000/09/xmldsig#");

                if(tagId)
                {
                    XmlAttribute xmlAttribute = doc.CreateAttribute("Id");
                    xmlAttribute.Value = "Ass_" + id.Replace(":", "_");
                    xmlSignature.Attributes.InsertAfter(xmlAttribute, xmlSignature.GetAttributeNode("xmlns"));
                }

                XmlElement xmlSignedInfo = signedXml.SignedInfo.GetXml();
                XmlElement xmlKeyInfo = signedXml.KeyInfo.GetXml();
                XmlElement xmlSignatureValue = doc.CreateElement("SignatureValue", xmlSignature.NamespaceURI);
                string signBase64 = Convert.ToBase64String(signedXml.Signature.SignatureValue);

                XmlText xmlText = doc.CreateTextNode(signBase64);
                xmlSignatureValue.AppendChild(xmlText);

                xmlSignature.AppendChild(doc.ImportNode(xmlSignedInfo, true));
                xmlSignature.AppendChild(xmlSignatureValue);
                xmlSignature.AppendChild(doc.ImportNode(xmlKeyInfo, true));

                if(assinarTagRps)
                {
                    infNfse.ParentNode.ParentNode.AppendChild(xmlSignature);
                }
                else
                {
                    infNfse.ParentNode.AppendChild(xmlSignature);
                }
            }

            String conteudoXmlAssinado = XmlDocumentToString(doc);
            return conteudoXmlAssinado;
        }

       public static string AssinarXML(string conteudoXML, System.Security.Cryptography.X509Certificates.X509Certificate2 certificado)
       {
            XmlDocument doc = new XmlDocument();
            System.Security.Cryptography.RSACryptoServiceProvider key = new System.Security.Cryptography.RSACryptoServiceProvider();

            System.Security.Cryptography.Xml.SignedXml signedDocument;
            System.Security.Cryptography.Xml.KeyInfo keyInfo = new System.Security.Cryptography.Xml.KeyInfo();

            doc.LoadXml(conteudoXML);

            //Retira chave privada ligada ao certificado
            key = (System.Security.Cryptography.RSACryptoServiceProvider)certificado.PrivateKey;

            //Adiciona Certificado ao Key Info
            keyInfo.AddClause(new System.Security.Cryptography.Xml.KeyInfoX509Data(certificado));
            signedDocument = new System.Security.Cryptography.Xml.SignedXml(doc);
            //Seta chaves
            signedDocument.SigningKey = key;
            signedDocument.KeyInfo = keyInfo;

            //Cria referencia
            System.Security.Cryptography.Xml.Reference reference = new System.Security.Cryptography.Xml.Reference();
            reference.Uri = string.Empty;

            //Adiciona transformacao a referencia
            reference.AddTransform(new System.Security.Cryptography.Xml.XmlDsigEnvelopedSignatureTransform());
            reference.AddTransform(new System.Security.Cryptography.Xml.XmlDsigC14NTransform(false));

            //Adiciona referencia ao xml
            signedDocument.AddReference(reference);

            //Calcula Assinatura
            signedDocument.ComputeSignature();

            //Pega representacao da assinatura
            System.Xml.XmlElement xmlDigitalSignature = signedDocument.GetXml();

            //Adiciona ao doc XML
            doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));

            return doc.OuterXml;
       }

        public static XmlDocument StringToXmlDocument(String dados)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(new StringReader(dados));

            return doc;
        }

        public static String XmlDocumentToString(XmlDocument xmlDoc)
        {
	        StringWriter sw = new StringWriter();
	        XmlTextWriter xtw = new XmlTextWriter(sw);
	        xmlDoc.WriteTo(xtw);

	        return sw.ToString();
        }

        public static string SignAssinaturaHash(X509Certificate2 cert, string assinatura)
        {
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa = cert.PrivateKey as RSACryptoServiceProvider;

            byte[] sAssinaturaByte = enc.GetBytes(assinatura);

            RSAPKCS1SignatureFormatter rsaf = new RSAPKCS1SignatureFormatter(rsa);
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            byte[] hash;
            hash = sha1.ComputeHash(sAssinaturaByte);

            rsaf.SetHashAlgorithm("SHA1");
            sAssinaturaByte = rsaf.CreateSignature(hash);

            string convertido = string.Empty;
            convertido = Convert.ToBase64String(sAssinaturaByte);

            return convertido;
        }
	}
}
