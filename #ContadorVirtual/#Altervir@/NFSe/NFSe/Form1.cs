using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
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

            string pathXml = string.Empty;
            string xmlAssinado = string.Empty;
            List<SP.RespostaEnvioConsulta> listaResposta = new List<SP.RespostaEnvioConsulta>();
            XmlDocument doc = new XmlDocument();

            if (rbTesteEnvioLoteRps.Checked)
            {
                //----------------------------------
                //OBJETO Nota PRA CONVERSÃO EM NFS-e
                //----------------------------------
                Nota nota = new Nota();
                nota.numeroLote = "1";
                nota.cnpjPrestador = "11111111111111";
                nota.IMPrestador = "12345678";
                nota.quantidadeRps = "1";
                nota.numeroNota = "1";
                nota.serieNota = "RPS";
                nota.tipoRps = "1";
                nota.dataEmissao = "2016-01-20T00:00:00";
                nota.naturezaOperacao = "1";
                nota.optanteSimples = "2";
                nota.status = "1"; //1 - ATIVA; 2 - CANCELADA
                nota.valorNota = "10.00";
                nota.deduzPIS = "2";
                nota.deduzCOFINS = "2";
                nota.deduzCSLL = "2";
                nota.deduzINSS = "2";
                nota.deduzIR = "2";
                //------------------
                nota.deduzISS = "2";
                //------------------
                nota.valorISS = "0.00";
                nota.valorLiquidoNota = "10.00";
                nota.itemListaServico = "0702";
                nota.discriminacaoServico = "SERVICO TESTE";
                nota.codigoIBGE = "3550308";
                nota.cpfCnpjTomador = "03259432906";
                nota.razaoSocialTomador = "ALTEVIR CARDOSO NETO";
                nota.endereco = "DERVIL MICHELIN";
                nota.numeroEndereco = "1385";
                nota.bairro = "JARDIM BRESSAN";
                nota.complemento = "CASA";
                nota.estado = "PR";
                nota.cep = "85913130";
                nota.telefoneTomador = "4532785386";
                nota.emailTomador = "altevir.cardoso@gmail.com";
                nota.aliquotaISS = "0.0500";
                //--------------------------

                pathXml = SP.GerarXml("D:\\PROJETOS\\NET\\NFSe\\TESTES\\SP\\LoteRps\\loteRps_" + nota.numeroLote + ".xml", nota, certificado);

                if (!string.IsNullOrEmpty(pathXml))
                {
                    doc.Load(pathXml);

                    xmlAssinado = CertificadoDigital.AssinarXML(doc.OuterXml, certificado);
                    doc.LoadXml(xmlAssinado);
                    doc.Save(pathXml);

                    listaResposta = SP.TransmitirServicos(certificado, pathXml, Nota.Metodos.testeEnviarLoteRps.ToString(), 1);

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

            if (rbConsultaCnpj.Checked)
            {
                string cnpjRemetente = "70306022000160";
                string cnpjContribuinte = "10174249000177";

                pathXml = SP.ConsultaCnpj(@"C:\#PROJETOS\#ContadorVirtual\#Altevir\TESTES\SP\Respostas_" + cnpjContribuinte + ".xml", cnpjRemetente, cnpjContribuinte);

                if (!string.IsNullOrEmpty(pathXml))
                {
                    doc.Load(pathXml);

                    xmlAssinado = CertificadoDigital.AssinarXML(doc.OuterXml, certificado);
                    doc.LoadXml(xmlAssinado);
                    doc.Save(pathXml);

                    listaResposta = SP.TransmitirServicos(certificado, pathXml, Nota.Metodos.consultarCnpj.ToString());


                }
            }
        }
    }
}
