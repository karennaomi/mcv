using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace NFSe
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
            X509Certificate2 certificado = new X509Certificate2();
            certificado = CertificadoDigital.Selecionar();

            if (certificado == null)
            {
                return;
            }

            //----------------------------------
            //OBJETO Nota PRA CONVERSÃO EM NFS-e
            //----------------------------------
            Nota nota = new Nota();
            nota.numeroLote = "18";
            nota.cnpjPrestador = "70306022000160";
            nota.IMPrestador = "43100899";
            nota.quantidadeRps = "1";
            nota.numeroNota = "18";
            nota.serieNota = "RPS";
            nota.tipoRps = "1";
            nota.dataEmissao = "2016-01-21T00:00:00";
            nota.naturezaOperacao = "1";
            nota.optanteSimples = "4";
            nota.status = "1"; //1 - ATIVA; 2 - CANCELADA
            nota.valorNota = "7663.68";
            nota.deduzPIS = "2";
            nota.deduzCOFINS = "2";
            nota.deduzCSLL = "2";
            nota.deduzINSS = "2";
            nota.deduzIR = "2";
            //------------------
            nota.deduzISS = "2";
            //------------------
            nota.valorISS = "0.00";
            nota.valorLiquidoNota = "7663.68";
            nota.itemListaServico = "03158";
            nota.discriminacaoServico = "Prestação de serviços ref mês 12/2015";
            nota.codigoIBGE = "3550308";
            nota.cpfCnpjTomador = "12423713000147";
            nota.razaoSocialTomador = "BLUE & YOU TREINAMENTOS EMPRESARIAS LTDA. -ME";
            nota.endereco = "R. LUISINIA,";
            nota.numeroEndereco = "00215";
            nota.bairro = "BROOKLIN PAULISTA";
            nota.IMTomador = "41248023";
            nota.complemento = "AP 22";
            nota.estado = "SP";
            nota.cep = "04660020";
            nota.telefoneTomador = "23945631";
            nota.emailTomador = "marcio.iavelberg@bluenumbers.com.br";
            nota.aliquotaISS = "0.05";
            //--------------------------

            //string pathXml = SP.GerarXml(@"C:\#PROJETOS\#ContadorVirtual\#Altevir\NFSe\Nota\loteRps_" + nota.numeroLote + ".xml", nota, certificado);
            string pathXml = SP.ConsultarLotePeriodo(@"C:\#PROJETOS\#ContadorVirtual\#Altevir\NFSe\Nota\loteRps_" + nota.numeroLote + ".xml", nota.cnpjPrestador, nota.cnpjPrestador, "2015-12-10", "2016-01-01","1", nota.IMPrestador);
            List<SP.RespostaEnvioConsulta> listaResposta = new List<SP.RespostaEnvioConsulta>();
            List<SP.RespostaEnvioConsulta> listaRespostaV = new List<SP.RespostaEnvioConsulta>();

            //string pathXml = SP.CancelarNfse(@"C:\#PROJETOS\#ContadorVirtual\#Altevir\NFSe\Nota\NotaCancelada_" + nota.numeroLote + ".xml", nota.cnpjPrestador,nota.IMPrestador,nota.numeroNota,  certificado);

            // string pathXmlConsulta = SP.ConsultarCNPJ("");
            if (!string.IsNullOrEmpty(pathXml))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(pathXml);

                string xmlAssinado = string.Empty;
                xmlAssinado = CertificadoDigital.AssinarXML(doc.OuterXml, certificado);
                doc.LoadXml(xmlAssinado);
                doc.Save(pathXml);

                //listaResposta = SP.TransmitirServicos(certificado, pathXml, Nota.Metodos.testeEnviarLoteRps.ToString(), 1);

                //listaRespostaV = SP.TransmitirServicos(certificado, pathXml, Nota.Metodos.cancelarNfse.ToString(), 1);

                //listaRespostaV = SP.TransmitirServicos(certificado, pathXml, Nota.Metodos.recepcionarLoteRps.ToString(), 1);

                listaRespostaV = SP.TransmitirServicos(certificado, pathXml, Nota.Metodos.consultarPeriodo.ToString(), 1);

                var resp = SP.TransmitirServicos(certificado, pathXml, Nota.Metodos.consultaCNPJ.ToString(),1);
                

                if (listaResposta.Count > 0)
                {
                    string mensagem = string.Empty;

                    if (!string.IsNullOrEmpty(listaResposta[0].descricaoErro))
                    {
                        foreach (var item in listaResposta)
                        {
                            mensagem += item.descricaoErro;
                        }
                    }

                    txtResposta.Text = mensagem;
                }
            }
        }
    }
}
