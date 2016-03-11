using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace NFSe
{
    class SP
    {
        #region ESTRUTURA PRA RESPOSTA

        public struct RespostaEnvioConsulta
        {
            public string numeroLoteGerado { get; set; }
            public string numeroLoteRecebido { get; set; }
            public string dataRecebimento { get; set; }
            public string dataEmissaoNfse { get; set; }
            public string situacaoLote { get; set; }
            public string descricaoErro { get; set; }
            public string descricaoAlerta { get; set; }
            public string numeroNfse { get; set; }
            public string statusNfse { get; set; }
            public string codigoVerificacao { get; set; }
            public string quantNotasProcessadas { get; set; }
            public string numeroRps { get; set; }
            public string serieRps { get; set; }
            public string sucesso { get; set; }
            public string dataHoraCancelamento { get; set; }
        }

        #endregion

        /// <summary>
        /// Método responsável por gerar o xml.
        /// </summary>
        /// <param name="pathXml">Diretório onde serão salvos os arquivos +  nome do arquivo a ser gerado.</param>
        /// <param name="nota">Objeto com os dados da Nota Fiscal (Rps).</param>
        /// <returns>Retorna uma string com o caminho do arquivo gerado.</returns>
        public static string GerarXml(string pathXml, Nota nfse, X509Certificate2 certificado)
        {
            Nota nota = new Nota();
            nota = nfse;

            try
            {
                if (System.IO.File.Exists(pathXml))
                {
                    System.IO.File.Delete(pathXml);
                }

                XmlTextWriter xml = new XmlTextWriter(pathXml, System.Text.Encoding.UTF8);
                xml.Formatting = Formatting.Indented;

                xml.WriteStartDocument();
                xml.WriteStartElement("PedidoEnvioLoteRPS");
                xml.WriteAttributeString("xmlns", "http://www.prefeitura.sp.gov.br/nfe");
                xml.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                xml.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xml.WriteStartElement("Cabecalho");
                xml.WriteAttributeString("Versao", "1");
                xml.WriteAttributeString("xmlns", "");
                xml.WriteStartElement("CPFCNPJRemetente");
                xml.WriteElementString("CNPJ", nota.cnpjPrestador);
                xml.WriteEndElement(); //fim elemento CPFCNPJRemetente
                xml.WriteElementString("transacao", "false");
                xml.WriteElementString("dtInicio", nota.dataEmissao.GetSubstring(0, 10));
                xml.WriteElementString("dtFim", nota.dataEmissao.GetSubstring(0, 10));
                xml.WriteElementString("QtdRPS", "1");
                xml.WriteElementString("ValorTotalServicos", nota.valorNota);
                xml.WriteElementString("ValorTotalDeducoes", "0.00");
                xml.WriteEndElement(); //fim elemento Cabecalho

                xml.WriteStartElement("RPS");
                xml.WriteAttributeString("xmlns", string.Empty);

                //--------------------------
                //GERAÇÃO DA ASSINATURA HASH
                //--------------------------
                string assinatura = string.Empty;
                string prestadorInscricaoMunicipal = string.Empty;
                string rpsSerie = string.Empty;
                string rpsNumero = string.Empty;
                string rpsDataEmissao = string.Empty;
                string rpsTributacao = string.Empty;
                string rpsTipoTributacao = string.Empty;
                string rpsStatus = string.Empty;
                string rpsISSRetido = string.Empty;
                string rpsValorServicos = string.Empty;
                string rpsValorDeducoes = string.Empty;
                string rpsCodigoServico = string.Empty;
                string tomadorTipoCpfCnpj = string.Empty;
                string tomadorCpfCnpj = string.Empty;

                prestadorInscricaoMunicipal = nota.IMPrestador.PadLeft(8, '0'); //tamanho 8
                rpsSerie = nota.serieNota.PadRight(5, ' '); //tamanho 5
                rpsNumero = nota.numeroNota.PadLeft(12, '0'); //tamanho 12
                rpsDataEmissao = string.Format("{0:yyyyMMdd}", Convert.ToDateTime(nota.dataEmissao)); //AAAAMMDD

                //-----------------
                //TRIBUTAÇÃO DO RPS
                //-----------------
                //T - TRIBUTADO NO MUNICIPIO DE SP
                //F - TRIBUTADO FORA DO MUNICIPIO DE SP
                //I - ISENTO
                //J - ISS SUSPENSO POR DECISAO JUDICIAL
                //-------------------------------------
                switch (nota.naturezaOperacao)
                {
                    case "1": rpsTributacao = "T"; break;
                    case "2": rpsTributacao = "F"; break;
                    case "3": rpsTributacao = "I"; break;
                    case "4": rpsTributacao = "J"; break;
                }

                rpsStatus = nota.status == "1" ? "N" : "C"; //N - NORNAL ; C - CANCELADO ; E - EXTRAVIADO
                rpsISSRetido = nota.deduzISS == "1" ? "S" : "N"; //S - SIM ; N - NAO
                rpsValorServicos = nota.valorNota.Replace(".", "").PadLeft(15, '0'); //tamanho 15
                rpsValorDeducoes = "000000000000000"; //tamanho 15
                rpsCodigoServico = nota.itemListaServico.PadLeft(5, '0'); //tamanho 5
                tomadorTipoCpfCnpj = nota.cpfCnpjTomador.Trim().Length == 11 ? "1" : "2";
                tomadorCpfCnpj = nota.cpfCnpjTomador.PadLeft(14, '0');

                assinatura = prestadorInscricaoMunicipal + rpsSerie + rpsNumero + rpsDataEmissao + rpsTributacao + rpsStatus + rpsISSRetido + rpsValorServicos + rpsValorDeducoes + rpsCodigoServico + tomadorTipoCpfCnpj + tomadorCpfCnpj;

                xml.WriteElementString("Assinatura", CertificadoDigital.SignAssinaturaHash(certificado, assinatura));
                //---------------------------------------------------------------------------------------------------

                xml.WriteStartElement("ChaveRPS");
                xml.WriteElementString("InscricaoPrestador", nota.IMPrestador.GetSubstring(0, 8)); //Inscricao Municipal Prestador
                xml.WriteElementString("SerieRPS", nota.serieNota); //serie do rps
                xml.WriteElementString("NumeroRPS", nota.numeroNota); //serie do rps
                xml.WriteEndElement(); //fim elemento ChaveRPS

                //--------
                //TIPO RPS
                //--------
                //RPS - RECIBO PROVISORIO DE SERVIÇOS (FIXO)
                //RPS-M - RECIBO PROVISORIO DE SERVICOS PROVENIENTE DE NOTA FISCAL CONJUGADA (MISTA)
                //RPS-C - CUPOM
                //-------------
                xml.WriteElementString("TipoRPS", "RPS");
                //---------------------------------------

                xml.WriteElementString("DataEmissao", nota.dataEmissao.GetSubstring(0, 10)); //data emissao do rps (AAAA-MM-DD)

                //-------------
                //STATUS DO RPS
                //-------------
                //N - NORMAL
                //C - CANCELADA
                //E - EXTRAVIADA
                //--------------
                xml.WriteElementString("StatusRPS", nota.status.Equals("1") ? "N" : "C");
                //-----------------------------------------------------------------------

                //-----------------
                //TRIBUTACAO DO RPS
                //-----------------
                //T - TRIBUTADO NO MUNICIPIO DE SP
                //F - TRIBUTADO FORA DO MUNICIPIO DE SP
                //I - ISENTO
                //J - ISS SUSPENSO POR DECISAO JUDICIAL
                //-------------------------------------
                switch (nota.naturezaOperacao)
                {
                    case "1": xml.WriteElementString("TributacaoRPS", "T"); break;
                    case "2": xml.WriteElementString("TributacaoRPS", "F"); break;
                    case "3": xml.WriteElementString("TributacaoRPS", "I"); break;
                    case "4": xml.WriteElementString("TributacaoRPS", "J"); break;
                }
                //----------------------------------------------------------------

                xml.WriteElementString("ValorServicos", nota.valorNota);
                xml.WriteElementString("ValorDeducoes", "0.00");

                //----------------------------------------------------------------------------------------
                //Para os impostos preencher somente se houver retenção, caso contrário enviar como "0.00"
                //----------------------------------------------------------------------------------------
                xml.WriteElementString("ValorPIS", nota.deduzPIS.Equals("1") ? nota.valorPIS : "0.00");
                xml.WriteElementString("ValorCOFINS", nota.deduzCOFINS.Equals("1") ? nota.valorCOFINS : "0.00");
                xml.WriteElementString("ValorINSS", nota.deduzINSS.Equals("1") ? nota.valorINSS : "0.00");
                xml.WriteElementString("ValorIR", nota.deduzIR.Equals("1") ? nota.valorIR : "0.00");
                xml.WriteElementString("ValorCSLL", nota.deduzCSLL.Equals("1") ? nota.valorCSLL : "0.00");
                xml.WriteElementString("CodigoServico", nota.itemListaServico); //CODIGO DO SERVICO PRESTADO
                xml.WriteElementString("AliquotaServicos", nota.aliquotaISS); //ALIQUOTA ISS - formato: 0.0500
                //-----------------------------------------------------------
                //----------
                //ISS Retido
                //----------
                //1 - true
                //2 - false
                //---------
                if (nota.deduzISS.Equals("1"))
                {
                    xml.WriteElementString("ISSRetido", "true");
                }
                else
                {
                    xml.WriteElementString("ISSRetido", "false");
                }
                //-----------------------------------------------

                xml.WriteStartElement("CPFCNPJTomador");
                if (nota.cpfCnpjTomador.Length == 11)
                {
                    xml.WriteElementString("CPF", nota.cpfCnpjTomador);
                }
                else
                {
                    xml.WriteElementString("CNPJ", nota.cpfCnpjTomador);
                }
                xml.WriteEndElement(); //fim elemento CPFCNPJTomador

                xml.WriteElementString("RazaoSocialTomador", nota.razaoSocialTomador.GetSubstring(0, 75));
                xml.WriteStartElement("EnderecoTomador");

                if (nota.endereco.ToUpper().Contains("RUA"))
                {
                    xml.WriteElementString("TipoLogradouro", "RUA");

                }
                else if (nota.endereco.ToUpper().Contains("AVENIDA") ||
                         nota.endereco.ToUpper().Contains("AV") ||
                         nota.endereco.ToUpper().Contains("AV."))
                {
                    xml.WriteElementString("TipoLogradouro", "AV");
                }

                xml.WriteElementString("Logradouro", nota.endereco.GetSubstring(0, 50));
                xml.WriteElementString("NumeroEndereco", nota.numeroEndereco.GetSubstring(0, 10));

                if (!string.IsNullOrEmpty(nota.complemento))
                {
                    xml.WriteElementString("ComplementoEndereco", nota.complemento.GetSubstring(0, 30));
                }

                xml.WriteElementString("Bairro", nota.bairro.GetSubstring(0, 30));
                xml.WriteElementString("Cidade", nota.codigoIBGE); //CODIGO IBGE DO TOMADOR
                xml.WriteElementString("UF", nota.estado);
                xml.WriteElementString("CEP", nota.cep);
                xml.WriteEndElement(); //fim elemento EnderecoTomador
                xml.WriteElementString("EmailTomador", nota.emailTomador.GetSubstring(0, 60));
                xml.WriteElementString("Discriminacao", nota.discriminacaoServico.GetSubstring(0, 2000));
                xml.WriteEndElement(); //fim elemento RPS

                xml.Close();
                nota = null;

                return pathXml;
            }
            catch (Exception erro)
            {
                nota = null;
                throw (new Exception(erro.Source.ToString() + "-" + erro.Message.ToString() + "\n" + erro.StackTrace.ToString()));
            }
        }

        /// <summary>
        /// Método responsável por efetuar a consulta do lote Rps.
        /// </summary>
        /// <param name="pathXml">Diretório onde serão salvos os arquivos +  nome do arquivo a ser gerado.</param>
        /// <param name="cnpj">CNPJ do Prestador.</param>
        /// <param name="numeroLote">Número do lote a ser consultado.</param>
        /// <returns>Retorna o nome do arquivo gerado.</returns>
        public static string ConsultarLoteRps(string pathXml, string cnpj, string numeroLote)
        {
            try
            {
                if (System.IO.File.Exists(pathXml))
                {
                    System.IO.File.Delete(pathXml);
                }

                XmlTextWriter xml = new XmlTextWriter(pathXml, System.Text.Encoding.UTF8);
                xml.Formatting = Formatting.Indented;

                xml.WriteStartDocument();
                xml.WriteStartElement("PedidoConsultaLote");
                xml.WriteAttributeString("xmlns", "http://www.prefeitura.sp.gov.br/nfe");
                xml.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xml.WriteStartElement("Cabecalho");
                xml.WriteAttributeString("Versao", "1");
                xml.WriteAttributeString("xmlns", "");
                xml.WriteStartElement("CPFCNPJRemetente");
                xml.WriteElementString("CNPJ", cnpj);
                xml.WriteEndElement(); //fim elemento CPFCNPJRemetente
                xml.WriteElementString("NumeroLote", numeroLote);
                xml.WriteEndElement(); //fim elemento Cabecalho
                xml.WriteEndElement(); //fim elemento PedidoConsultaLote
                xml.Close();

                return pathXml;
            }
            catch (Exception erro)
            {
                throw (new Exception(erro.Source.ToString() + "-" + erro.Message.ToString()));
            }
        }

        /// <summary>
        /// Método responsável por efetuar a consulta da Nfs-e pelo número do Rps.
        /// </summary>
        /// <param name="pathXml">Diretório onde serão salvos os arquivos +  nome do arquivo a ser gerado.</param>
        /// <param name="cnpj">CNPJ do prestador.</param>
        /// <param name="inscricaoMunicipal">Número da Inscrição Municipal do Prestador.</param>
        /// <param name="numeroNfse">Número da Nfs-e.</param>
        /// <returns>Retorna o nome do arquivo gerado.</returns>
        public static string ConsultarNfse(string pathXml, string cnpj, string inscricaoMunicipal, string numeroRps, string serieRps)
        {
            try
            {
                if (System.IO.File.Exists(pathXml))
                {
                    System.IO.File.Delete(pathXml);
                }

                XmlTextWriter xml = new XmlTextWriter(pathXml, System.Text.Encoding.UTF8);
                xml.Formatting = Formatting.Indented;

                xml.WriteStartElement("PedidoConsultaNFe");
                xml.WriteAttributeString("xmlns", "http://www.prefeitura.sp.gov.br/nfe");
                xml.WriteStartElement("Cabecalho");
                xml.WriteAttributeString("Versao", "1");
                xml.WriteAttributeString("xmlns", "");
                xml.WriteStartElement("CPFCNPJRemetente");
                xml.WriteElementString("CNPJ", cnpj);
                xml.WriteEndElement(); //fim elemento CPFCNPJRemetente
                xml.WriteEndElement(); //fim elemento Cabecalho
                xml.WriteStartElement("Detalhe");
                xml.WriteAttributeString("xmlns", "");
                xml.WriteStartElement("ChaveRPS");
                xml.WriteElementString("InscricaoPrestador", inscricaoMunicipal.GetSubstring(0, 8).PadLeft(8, '0'));
                xml.WriteElementString("SerieRPS", serieRps);
                xml.WriteElementString("NumeroRPS", numeroRps);
                xml.WriteEndElement(); //fim elementro ChaveRPS
                xml.WriteEndElement(); //fim elemento Detalhe
                xml.WriteEndElement(); //fim elemento PedidoConsultaNFe
                xml.Close();

                return pathXml;
            }
            catch (Exception erro)
            {
                throw (new Exception(erro.Source.ToString() + "-" + erro.Message.ToString()));
            }
        }

        /// <summary>
        /// Método responsável por efetuar o cancelamento da Nfs-e.
        /// </summary>
        /// <param name="pathXml">Diretório onde serão salvos os arquivos +  nome do arquivo a ser gerado.</param>
        /// <param name="cnpj">CNPJ do prestador.</param>
        /// <param name="inscMunicipal">Número da Inscrição Municipal do Prestador.</param>
        /// <param name="numroNfse">Número da Nfs-e.</param>
        /// <param name="certificado">Descrição com o motivo do cancelamento da Nfs-e.</param>
        /// <returns></returns>
        public static string CancelarNfse(string pathXml, string cnpj, string inscMunicipal, string numroNfse, X509Certificate2 certificado)
        {
            try
            {
                if (System.IO.File.Exists(pathXml))
                {
                    System.IO.File.Delete(pathXml);
                }

                XmlTextWriter xml = new XmlTextWriter(pathXml, System.Text.Encoding.UTF8);
                xml.Formatting = Formatting.Indented;
                xml.WriteStartDocument();

                xml.WriteStartElement("PedidoCancelamentoNFe");
                xml.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xml.WriteAttributeString("xmlns", "http://www.prefeitura.sp.gov.br/nfe");
                xml.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                xml.WriteStartElement("Cabecalho");
                xml.WriteAttributeString("Versao", "1");
                xml.WriteAttributeString("xmlns", "");
                xml.WriteStartElement("CPFCNPJRemetente");
                xml.WriteElementString("CNPJ", cnpj);
                xml.WriteEndElement(); //fim elemento CPFCNPJRemetente
                xml.WriteElementString("transacao", "true");
                xml.WriteEndElement(); //fim elemento Cabecalho
                xml.WriteStartElement("Detalhe");
                xml.WriteAttributeString("xmlns", "");
                xml.WriteStartElement("ChaveNFe");
                xml.WriteElementString("InscricaoPrestador", inscMunicipal.GetSubstring(0, 8).PadLeft(8, '0'));
                xml.WriteElementString("NumeroNFe", numroNfse);
                xml.WriteEndElement(); //fim elementro ChaveNFe
                xml.WriteElementString("AssinaturaCancelamento", CertificadoDigital.SignAssinaturaHash(certificado, inscMunicipal.GetSubstring(0, 8).PadLeft(8, '0') + numroNfse.PadLeft(12, '0')));
                xml.WriteEndElement(); //fim elemento Detalhe
                xml.Close();

                return pathXml;
            }
            catch (Exception erro)
            {
                throw (new Exception(erro.Source.ToString() + "-" + erro.Message.ToString()));
            }
        }

        /// <summary>
        /// Método responsável por efetuar o consulta do CNPJ do contribuinte.
        /// </summary>
        /// <param name="pathXml">Diretório onde serão salvos os arquivos +  nome do arquivo a ser gerado.</param>
        /// <param name="cnpjRemetente">CNPJ do remetente.</param>
        /// <param name="cnpjContribuinte">CNPJ do contribuinte.</param>
        /// <returns></returns>
        public static string ConsultaCnpj(string pathXml, string cnpjRemetente, string cnpjContribuinte)
        {
            try
            {
                if (System.IO.File.Exists(pathXml))
                {
                    System.IO.File.Delete(pathXml);
                }

                XmlTextWriter xml = new XmlTextWriter(pathXml, System.Text.Encoding.UTF8);
                xml.Formatting = Formatting.Indented;

                xml.WriteStartDocument();
                xml.WriteStartElement("p1:PedidoConsultaCNPJ");
                xml.WriteAttributeString("xmlns:p1", "http://www.prefeitura.sp.gov.br/nfe");
                xml.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xml.WriteStartElement("Cabecalho");
                xml.WriteAttributeString("Versao", "1");
                xml.WriteStartElement("CPFCNPJRemetente");
                xml.WriteElementString("CNPJ", cnpjRemetente);
                xml.WriteEndElement(); //fim elemento CPFCNPJRemetente
                xml.WriteEndElement(); //fim elemento Cabecalho
                xml.WriteStartElement("CNPJContribuinte");
                xml.WriteElementString("CNPJ", cnpjContribuinte);
                xml.WriteEndElement();
                xml.WriteEndElement(); //fim elemento PedidoConsultaLote
                xml.Close();

                return pathXml;
            }
            catch (Exception erro)
            {
                throw (new Exception(erro.Source.ToString() + "-" + erro.Message.ToString()));
            }
        }

        /// <summary>
        /// Método responsável por fazer a transmissão/requisições dos serviços ao WebService da Prefeitura de São Paulo-SP.
        /// </summary>
        /// <param name="certificado">Certificado Digital do Prestador.</param>
        /// <param name="pathXml">Diretório onde serão salvos os arquivos +  nome do arquivo a ser gerado.</param>
        /// <param name="metodo">Nome do método a ser executado no WebService (exemplo: recepcionarLoteRps).</param>
        /// <param name="numLote">Número do Lote.</param>
        /// <param name="numNfse">Informar somente no Cancelamento da Nfs-e, caso contrário passar em branco.</param>
        /// <returns>Retorna uma lista com as informações do Lote e/ou da Nfse.</returns>
        /// 
        public static List<RespostaEnvioConsulta> TransmitirServicos(X509Certificate2 certificado, string pathXml, string metodo, int numLote = 0, string numNfse = "")
        {
            List<RespostaEnvioConsulta> lista = new List<RespostaEnvioConsulta>();
            string pathRespostas = "";
            string urlWebService = "https://nfe.prefeitura.sp.gov.br/ws/lotenfe.asmx?WSDL";
            string respostaWebService = "";

            try
            {
                //-----------------------
                //LEITURA DO XML ASSINADO
                //-----------------------
                XmlDocument xmlAssinado = new XmlDocument();
                xmlAssinado.Load(pathXml);
                //-----------------------------

                LoteNFe ws = new LoteNFe();
                ws.Url = urlWebService;
                ws.ClientCertificates.Add(certificado);
                ws.Timeout = 200000;

                switch (metodo)
                {
                    case "testeEnviarLoteRps":
                        respostaWebService = ws.TesteEnvioLoteRPS(1, xmlAssinado.OuterXml);
                        break;
                    case "recepcionarLoteRps":
                        respostaWebService = ws.EnvioLoteRPS(1, xmlAssinado.OuterXml);
                        break;

                    case "consultarLoteRps":
                        respostaWebService = ws.ConsultaLote(1, xmlAssinado.OuterXml);
                        break;

                    case "consultarNfse":
                        respostaWebService = ws.ConsultaNFe(1, xmlAssinado.OuterXml);
                        break;

                    case "cancelarNfse":
                        respostaWebService = ws.CancelamentoNFe(1, xmlAssinado.OuterXml);
                        break;

                    case "consultarCnpj":
                        respostaWebService = ws.ConsultaCNPJ(1, xmlAssinado.OuterXml);
                        break;
                }

                string arquivoResposta = @"C:\#PROJETOS\#ContadorVirtual\#Altevir\TESTES\SP\Respostas\Resp_" + System.IO.Path.GetFileName(pathXml);

                if (!System.IO.Directory.Exists(pathRespostas))
                {
                    System.IO.Directory.CreateDirectory(pathRespostas);
                }

                XmlDocument xmlResposta = new XmlDocument();
                xmlResposta.LoadXml(respostaWebService);
                xmlResposta.Save(arquivoResposta);

                if (System.IO.File.Exists(arquivoResposta))
                {
                    lista = SP.ProcessarRespostas(pathXml, arquivoResposta, metodo, numLote, numNfse);
                }
                else
                {
                    return lista;
                }

                return lista;
            }
            catch (Exception erro)
            {
                throw (new Exception(erro.Source.ToString() + "-" + erro.Message.ToString() + "\n" + erro.StackTrace.ToString()));
            }
        }

        /// <summary>
        /// Método responsável por armazenar os dados do envio do lote e informações das consultas e da Nfs-e.
        /// </summary>
        /// <param name="arquivoResposta">Diretório + Nome do arquivo xml de resposta.</param>
        /// <param name="metodo">Nome do método que foi enviado ao WebService.</param>
        /// <returns>Retorna uma lista com o Objeto preenchido para gravar no BD.</returns>
        public static List<RespostaEnvioConsulta> ProcessarRespostas(string arquivoEnvio, string arquivoResposta, string metodo, int numLoteGerado, string numNfse)
        {
            List<RespostaEnvioConsulta> lista = new List<RespostaEnvioConsulta>();
            RespostaEnvioConsulta resposta = new RespostaEnvioConsulta();

            DataSet ds = new DataSet();
            ds.ReadXml(arquivoResposta);

            if (ds.Tables.Contains("Cabecalho") && ds.Tables.Contains("InformacoesLote"))
            {
                resposta.numeroLoteRecebido = Funcoes.RetornarCampoXml(ds, "InformacoesLote", 0, "NumeroLote", false);
                resposta.dataRecebimento = Funcoes.RetornarCampoXml(ds, "InformacoesLote", 0, "DataEnvioLote", false).Replace("T", " ").GetSubstring(0, 19);
                resposta.sucesso = Funcoes.RetornarCampoXml(ds, "Cabecalho", 0, "Sucesso", false);
                resposta.quantNotasProcessadas = Funcoes.RetornarCampoXml(ds, "InformacoesLote", 0, "QtdNotasProcessadas", false);

                //------------------------------------
                //ALTERACAO DO NOME DO ARQUIVO ENVIADO
                //------------------------------------
                string arquivoEnvioAntigo = string.Empty;
                string arquivoEnvioNovo = string.Empty;
                string arquivoRespostaNovo = string.Empty;

                arquivoEnvioAntigo = arquivoEnvio;
                arquivoEnvioNovo = arquivoEnvio.Replace("NNN", resposta.numeroLoteRecebido);

                arquivoRespostaNovo = arquivoResposta.Replace("NNN", resposta.numeroLoteRecebido);

                System.IO.File.Move(arquivoEnvio, arquivoEnvioNovo);
                System.IO.File.Move(arquivoResposta, arquivoRespostaNovo);
                //--------------------------------------------------------

                if (resposta.sucesso == "false" && ds.Tables["Erro"].Rows.Count > 0)
                {
                    //-------------------
                    //PROCESSADO COM ERRO
                    //-------------------
                    for (int a = 0; a < ds.Tables["Erro"].Rows.Count; a++)
                    {
                        resposta = new RespostaEnvioConsulta();
                        resposta.numeroLoteGerado = numLoteGerado.ToString();
                        resposta.numeroLoteRecebido = Funcoes.RetornarCampoXml(ds, "InformacoesLote", 0, "NumeroLote", false);
                        resposta.dataRecebimento = Funcoes.RetornarCampoXml(ds, "InformacoesLote", 0, "DataEnvioLote", false).Replace("T", " ").GetSubstring(0, 19);
                        resposta.sucesso = Funcoes.RetornarCampoXml(ds, "Cabecalho", 0, "Sucesso", false);
                        resposta.quantNotasProcessadas = Funcoes.RetornarCampoXml(ds, "InformacoesLote", 0, "QtdNotasProcessadas", false);
                        resposta.situacaoLote = "Processado com Erro";

                        resposta.descricaoErro += "RPS: " + Funcoes.RetornarCampoXml(ds, "ChaveRPS", a, "NumeroRPS", false) + " - " + Funcoes.RetornarCampoXml(ds, "ChaveRPS", a, "SerieRPS", false) + " | ";
                        resposta.descricaoErro += Funcoes.RetornarCampoXml(ds, "Erro", a, "Codigo", false) + " | ";
                        resposta.descricaoErro += Funcoes.RetornarCampoXml(ds, "Erro", a, "Descricao", false);

                        lista.Add(resposta);
                    }

                    return lista;
                }
                else if (resposta.sucesso == "true" && resposta.quantNotasProcessadas == "0")
                {
                    //Nao Processado
                    //nao teve retorno com esse tipo de situacao, pode ser implementada futuramente.
                    return lista;
                }
                else if (resposta.sucesso == "true" && resposta.quantNotasProcessadas != "0")
                {
                    //---------------------------------
                    //Verifica se as notas tem Alertas!
                    //---------------------------------
                    string alertas = string.Empty;
                    List<String> listaAlertas = null;
                    if (ds.Tables.Contains("Alerta") && ds.Tables["Alerta"].Rows.Count > 0)
                    {
                        listaAlertas = new List<string>();
                        for (int a = 0; a < ds.Tables["Alerta"].Rows.Count; a++)
                        {
                            listaAlertas.Add("RPS: " + Funcoes.RetornarCampoXml(ds, "ChaveRPS", a, "NumeroRPS", false).PadLeft(10, '0') + " - " + Funcoes.RetornarCampoXml(ds, "ChaveRPS", a, "SerieRPS", false) + " | Alerta: " + Funcoes.RetornarCampoXml(ds, "Alerta", a, "Codigo", false) + " | " + Funcoes.RetornarCampoXml(ds, "Alerta", a, "Descricao", false));
                        }
                    }
                    //----------------------------------------------------------
                    //--------------------------------------
                    //PROCESSADO COM SUCESSO - NFS-e GERADAS
                    //--------------------------------------
                    for (int a = 0; a < ds.Tables["ChaveNFeRPS"].Rows.Count; a++)
                    {
                        resposta = new RespostaEnvioConsulta();
                        resposta.situacaoLote = "NFS-e Gerada";

                        if (!string.IsNullOrEmpty(alertas)) { resposta.descricaoAlerta = alertas; }

                        resposta.numeroLoteGerado = numLoteGerado.ToString();
                        resposta.numeroLoteRecebido = Funcoes.RetornarCampoXml(ds, "InformacoesLote", 0, "NumeroLote", false);
                        resposta.dataRecebimento = Funcoes.RetornarCampoXml(ds, "InformacoesLote", 0, "DataEnvioLote", false).Replace("T", " ").GetSubstring(0, 19);
                        resposta.numeroNfse = Funcoes.RetornarCampoXml(ds, "ChaveNFe", a, "NumeroNFe", false);
                        resposta.codigoVerificacao = Funcoes.RetornarCampoXml(ds, "ChaveNFe", a, "CodigoVerificacao", false);
                        resposta.numeroRps = Funcoes.RetornarCampoXml(ds, "ChaveRPS", a, "NumeroRPS", false);
                        resposta.serieRps = Funcoes.RetornarCampoXml(ds, "ChaveRPS", a, "SerieRPS", false);

                        if (listaAlertas != null)
                        {
                            if (listaAlertas.Count > 0)
                            {
                                for (int b = 0; b < listaAlertas.Count; b++)
                                {
                                    //verifica se o Alerta é correspondente ao RPS, se sim, vincula o Alerta ao RPS.
                                    //tratamento efetuado devido a poder ocorrer de vir somente um Alerta e o lote ter mais de uma nota.
                                    if (Convert.ToDecimal(listaAlertas[b].GetSubstring(6, 10)) == Convert.ToDecimal(resposta.numeroRps))
                                    {
                                        resposta.descricaoAlerta = listaAlertas[a].ToString();
                                    }
                                }
                            }
                        }

                        lista.Add(resposta);
                    }

                    return lista;
                }

                return lista;
            }
            else if (ds.Tables.Contains("Cabecalho") &&
                        ds.Tables.Contains("Erro") &&
                        metodo != "cancelarNfse")
            {
                if (ds.Tables.Contains("Erro") &&
                    ds.Tables["Erro"].Rows.Count > 0)
                {
                    //-------------------
                    //PROCESSADO COM ERRO
                    //-------------------
                    for (int a = 0; a < ds.Tables["Erro"].Rows.Count; a++)
                    {
                        resposta = new RespostaEnvioConsulta();
                        resposta.numeroLoteGerado = numLoteGerado.ToString();
                        resposta.situacaoLote = "Processado com Erro";
                        resposta.descricaoErro += Funcoes.RetornarCampoXml(ds, "Erro", a, "Codigo", false) + " | ";
                        resposta.descricaoErro += Funcoes.RetornarCampoXml(ds, "Erro", a, "Descricao", false);

                        lista.Add(resposta);
                    }
                    return lista;
                }

                //-------------------------------------------
                //ATUALIZA/VINCULA O NUMERO DO LOTE AOS RPS'S
                //-------------------------------------------
                //IMPLEMENTAR CONFORME REGRA DE NEGÓCIOS
                //--------------------------------------

                return lista;
            }
            else if (ds.Tables.Contains("Cabecalho") &&
                        ds.Tables.Contains("NFe") &&
                        ds.Tables["NFe"].Rows.Count > 0)
            {
                resposta = new RespostaEnvioConsulta();

                resposta.numeroNfse = Funcoes.RetornarCampoXml(ds, "ChaveNFe", 0, "NumeroNFe", false);
                resposta.codigoVerificacao = Funcoes.RetornarCampoXml(ds, "ChaveNFe", 0, "CodigoVerificacao", false);
                resposta.dataEmissaoNfse = Funcoes.RetornarCampoXml(ds, "NFe", 0, "DataEmissaoNFe", false).Replace("T", " ");
                resposta.statusNfse = Funcoes.RetornarCampoXml(ds, "NFe", 0, "StatusNFe", false);
                resposta.numeroRps = Funcoes.RetornarCampoXml(ds, "ChaveRPS", 0, "NumeroRPS", false);
                resposta.serieRps = Funcoes.RetornarCampoXml(ds, "ChaveRPS", 0, "SerieRPS", false);

                lista.Add(resposta);

                return lista;
            }
            else if (ds.Tables.Contains("Cabecalho") &&
                        metodo.Equals("cancelarNfse"))
            {
                //cancelamento da Nfse
                resposta = new RespostaEnvioConsulta();
                resposta.dataHoraCancelamento = DateTime.Now.ToString();
                resposta.numeroNfse = numNfse;
                resposta.sucesso = ds.Tables["Cabecalho"].Rows[0]["Sucesso"].ToString().Trim();

                if (ds.Tables.Contains("Alerta") && ds.Tables["Alerta"].Rows.Count > 0)
                {
                    for (int a = 0; a < ds.Tables["Alerta"].Rows.Count; a++)
                    {
                        resposta.descricaoAlerta += Funcoes.RetornarCampoXml(ds, "Alerta", a, "Codigo", false) + " | " + Funcoes.RetornarCampoXml(ds, "Alerta", a, "Descricao", false);
                    }

                }
                else if (ds.Tables.Contains("Erro") && ds.Tables["Erro"].Rows.Count > 0)
                {

                    for (int a = 0; a < ds.Tables["Erro"].Rows.Count; a++)
                    {
                        resposta.descricaoErro += Funcoes.RetornarCampoXml(ds, "Erro", a, "Codigo", false) + " | " + Funcoes.RetornarCampoXml(ds, "Erro", a, "Descricao", false);
                    }
                }

                lista.Add(resposta);

                return lista;
            }

            return lista;
        }
    }
}
