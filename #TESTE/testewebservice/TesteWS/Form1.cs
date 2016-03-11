using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.Xml;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace TesteWS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
//            var wfile = new System.IO.StreamWriter(@"C:\#PROJETOS\#TESTE\testewebservice\TesteWS\Pedido.xml");

            LoteNFe n = new LoteNFe();

            XmlDocument doc = new XmlDocument();
            doc.LoadXml("<?xml version='1.0' encoding='UTF-8'?><PedidoEnvioRPS xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns='http://www.prefeitura.sp.gov.br/nfe'>  <Cabecalho Versao='1' xmlns=''>    <CPFCNPJRemetente>      <CNPJ>04642554000143</CNPJ>    </CPFCNPJRemetente>  </Cabecalho>  <RPS xmlns=''>    <Assinatura>KGK4PDZOCZsnjVetsCZbq7Luy1ibESkRXLcx8TABxZGH+owti9rn4zbBFEnq1ZeRsi90WDaqa2V1wsNHFyRRfKeiBZ9yDRWQ0jLq3pYlIOqb8zVJxUFatOzzvv/zbAcvMbxg+pQH3r823akTI6dpZCdMv381zMX5xkS+AaFxoz9oHqC7+UJeZkejxgfVhjyWDCTd1mOTqcmTNDSopKdNoOwFSjyzNwrGFLFnK1klf1Oy0lzA4a1Q9XHa82ER0yB5zPM2kw/kUHknEIxbP0q7ztZ0XDZrGMw2S5+c8/Louv3KL8a2TL+Jjh9eTiixhiHyF3kBfbUQWQq5uZ4Cwv7OHw==</Assinatura>    <ChaveRPS>      <InscricaoPrestador>31000000</InscricaoPrestador>      <SerieRPS>AAAAA</SerieRPS>      <NumeroRPS>2</NumeroRPS>    </ChaveRPS>    <TipoRPS>RPS-M</TipoRPS>    <DataEmissao>2007-01-10</DataEmissao>    <StatusRPS>N</StatusRPS>    <TributacaoRPS>T</TributacaoRPS>    <ValorServicos>2050</ValorServicos>    <ValorDeducoes>500</ValorDeducoes>    <ValorPIS>1</ValorPIS>    <ValorCOFINS>1</ValorCOFINS>    <ValorINSS>1</ValorINSS>    <ValorIR>1</ValorIR>    <ValorCSLL>1</ValorCSLL>    <CodigoServico>2658</CodigoServico>    <AliquotaServicos>0.05</AliquotaServicos>    <ISSRetido>false</ISSRetido>    <CPFCNPJTomador>      <CPF>39521777176</CPF>    </CPFCNPJTomador>    <RazaoSocialTomador>TOMADOR PF</RazaoSocialTomador>    <EnderecoTomador>      <TipoLogradouro>Av</TipoLogradouro>      <Logradouro>Paulista</Logradouro>      <NumeroEndereco>100</NumeroEndereco>      <ComplementoEndereco>Cj 35</ComplementoEndereco>      <Bairro>Bela Vista</Bairro>      <Cidade>3550308</Cidade>      <UF>SP</UF>      <CEP>01310100</CEP>    </EnderecoTomador>    <EmailTomador>karennaomy@gmail.com</EmailTomador>    <Discriminacao>Desenvolvimento de Web Site Pessoal. (WS com Esquema ASCII) Discriminação com vários caracteres  _ = + | , ; : / ? ° º ª</Discriminacao> </RPS>  <Signature xmlns='http://www.w3.org/2000/09/xmldsig#'>    <SignedInfo>      <CanonicalizationMethod Algorithm='http://www.w3.org/TR/2001/REC-xml-c14n-20010315' />      <SignatureMethod Algorithm='http://www.w3.org/2000/09/xmldsig#rsa-sha1' />      <Reference URI=''>        <Transforms>          <Transform Algorithm='http://www.w3.org/2000/09/xmldsig#enveloped-signature' />          <Transform Algorithm='http://www.w3.org/TR/2001/REC-xml-c14n-20010315' />        </Transforms>        <DigestMethod Algorithm='http://www.w3.org/2000/09/xmldsig#sha1' />        <DigestValue>qKjVTKAe8rHZxLLFW9+ZVBdyRt8=</DigestValue>      </Reference>    </SignedInfo>    <SignatureValue>nLJdZQWuihKgwGV27ClGfA/J5IjiA4R960McZdnkWSHrqD2TXGSKI0NChLO1x2CNvHAWcV+66E1kmqhxSkk49G3KMG3jIsPYc+ca4S693CkBJPqvOur14FeJxegQ/0es1R1se/twnxpt/aOEQHvWzKRNVkvv35Pbty1CXQ1gWcU=</SignatureValue>    <KeyInfo>      <X509Data>        <X509Certificate>MIIFUzCCBDugAwIBAgIQSUJS8pELZyjasDkgGzKm0TANBgkqhkiG9w0BAQUFADBuMQswCQYDVQQGEwJCUjETMBEGA1UEChMKSUNQLUJyYXNpbDEsMCoGA1UECxMjU2VjcmV0YXJpYSBkYSBSZWNlaXRhIEZlZGVyYWwgLSBTUkYxHDAaBgNVBAMTE0FDIENlcnRpU2lnbiBTUkYgVjMwHhcNMDYwNzE5MDAwMDAwWhcNMDkwNzE4MjM1OTU5WjCB1DELMAkGA1UEBhMCQlIxEzARBgNVBAoUCklDUC1CcmFzaWwxKjAoBgNVBAsTIVNlY3JldGFyaWEgZGEgUmVjZWl0YSBGZWRlcmFsLVNSRjETMBEGA1UECxQKU1JGIGUtQ05QSjELMAkGA1UECBMCUkoxFzAVBgNVBAcUDlJJTyBERSBKQU5FSVJPMUkwRwYDVQQDE0BUSVBMQU4gQ09OU1VMVE9SSUEgRSBTRVJWSUNPUyBFTSBJTkZPUk1BVElDQSBMVERBOjA0NjQyNTU0MDAwMTQzMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCx86LAoJRVmtQMzmtdWpyNgKy200+bwjtz/TuywNcTjvfw7qHFGIgTjipmuZ3zhX28CgYLYXp3tj1Dfh2B7EhjHdLJPfvoF4MgbN/dQGXmGpMpF5cNxYusOGCZiyASvI7Gqt/xE4xLSIalNr6kF6CaPLkpFgTNNe+WQkG0fMqsQQIDAQABo4ICCDCCAgQwgbEGA1UdEQSBqTCBpqA/BgVgTAEDBKA2DDQyNDA3MTk3NjA3MTM4NTM3Nzg2MDAwMDAwMDAwMDAwMDAwMDAwOTI5OTA2MjFDTkggIFJKoB8GBWBMAQMCoBYMFEZFUk5BTkRPIFNJTFZBIEJSQUdBoBkGBWBMAQMDoBAMDjA0NjQyNTU0MDAwMTQzoBEGBWBMAQMHoAgMBjIzOTU0OIEUZmJyYWdhQHRpcGxhbi5jb20uYnIwCQYDVR0TBAIwADBiBgNVHR8EWzBZMFegVaBThlFodHRwOi8vaWNwLWJyYXNpbC5jZXJ0aXNpZ24uY29tLmJyL3JlcG9zaXRvcmlvL2xjci9BQ0NlcnRpU2lnblNSRlYzL0xhdGVzdENSTC5jcmwwHwYDVR0jBBgwFoAU9p1ZXf6/xXLN3c7ELmYbLu4Iz3YwDgYDVR0PAQH/BAQDAgXgMFUGA1UdIAROMEwwSgYGYEwBAgMGMEAwPgYIKwYBBQUHAgEWMmh0dHA6Ly9pY3AtYnJhc2lsLmNlcnRpc2lnbi5jb20uYnIvcmVwb3NpdG9yaW8vZHBjMB0GA1UdJQQWMBQGCCsGAQUFBwMEBggrBgEFBQcDAjA4BggrBgEFBQcBAQQsMCowKAYIKwYBBQUHMAGGHGh0dHA6Ly9vY3NwLmNlcnRpc2lnbi5jb20uYnIwDQYJKoZIhvcNAQEFBQADggEBAC5w/CBXAykvPSbBGf+u0UPcWVJATL2ix0hCfNUVtHaCjMz8hRjgYqmhpefzDm2LCTvoCPzG6XQBYxAmnDhX1f/gyjHz+E1xJg451qtqcyCJ9861o9R2bHd4zR0DuyxCNGOTiYJ4Gc/Xa4xqECorAx5ktkk1T/HOc1K/ntRGpdL+llsO/jqSRmTOnRgdeNHcKkyXsOgL5BwxxgGNuIyqirgGXW0by4Io1GnSXtixxfvEOnqOicxBY6AcVS9HHuhmOBYiK9skAUp0Sm2v41hpsC8uIkfUeRxsJIp2CNZ4DjoyfmKwNLMCRZQAKpwMXyyHZlX1a4o/9iGTszNeeShw61g=</X509Certificate>      </X509Data>    </KeyInfo>  </Signature></PedidoEnvioRPS>");
            //doc.LoadXml("<?xml version='1.0' encoding='UTF-8'?><PedidoEnvioRPS xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns='http://www.prefeitura.sp.gov.br/nfe'>  <Cabecalho Versao='1' xmlns=''>    <CPFCNPJRemetente>      <CNPJ>04642554000143</CNPJ>    </CPFCNPJRemetente>  </Cabecalho>  <RPS xmlns=''>    <Assinatura>lQllFyvbUo6C68CnFrFosa2jPbjmplQ6x3Q59+vYeTXAwACzXfD71aziAnP3NtEP/UiRRAyOQZOO73N+u2g8sanXJ4jhIOMXkP6yeK9JwTkZ/UoeJQUS7j7iyGw0IOq6o6sb0sV0BxZiclI/EMDvZ5H2zZrEEF9AstZkyEoEoJ8=</Assinatura>    <ChaveRPS>      <InscricaoPrestador>31000000</InscricaoPrestador>      <SerieRPS>AAAAA</SerieRPS>      <NumeroRPS>2</NumeroRPS>    </ChaveRPS>    <TipoRPS>RPS-M</TipoRPS>    <DataEmissao>2007-01-10</DataEmissao>    <StatusRPS>N</StatusRPS>    <TributacaoRPS>T</TributacaoRPS>    <ValorServicos>2050</ValorServicos>    <ValorDeducoes>500</ValorDeducoes>    <ValorPIS>1</ValorPIS>    <ValorCOFINS>1</ValorCOFINS>    <ValorINSS>1</ValorINSS>    <ValorIR>1</ValorIR>    <ValorCSLL>1</ValorCSLL>    <CodigoServico>2658</CodigoServico>    <AliquotaServicos>0.05</AliquotaServicos>    <ISSRetido>false</ISSRetido>    <CPFCNPJTomador>      <CPF>39521777176</CPF>    </CPFCNPJTomador>    <RazaoSocialTomador>TOMADOR PF</RazaoSocialTomador>    <EnderecoTomador>      <TipoLogradouro>Av</TipoLogradouro>      <Logradouro>Paulista</Logradouro>      <NumeroEndereco>100</NumeroEndereco>      <ComplementoEndereco>Cj 35</ComplementoEndereco>      <Bairro>Bela Vista</Bairro>      <Cidade>3550308</Cidade>      <UF>SP</UF>      <CEP>01310100</CEP>    </EnderecoTomador>    <EmailTomador>karennaomy@gmail.com</EmailTomador>    <Discriminacao>Desenvolvimento de Web Site Pessoal. (WS com Esquema ASCII) Discriminação com vários caracteres  _ = + | , ; : / ? ° º ª</Discriminacao> </RPS>  <Signature xmlns='http://www.w3.org/2000/09/xmldsig#'>    <SignedInfo>      <CanonicalizationMethod Algorithm='http://www.w3.org/TR/2001/REC-xml-c14n-20010315' />      <SignatureMethod Algorithm='http://www.w3.org/2000/09/xmldsig#rsa-sha1' />      <Reference URI=''>        <Transforms>          <Transform Algorithm='http://www.w3.org/2000/09/xmldsig#enveloped-signature' />          <Transform Algorithm='http://www.w3.org/TR/2001/REC-xml-c14n-20010315' />        </Transforms>        <DigestMethod Algorithm='http://www.w3.org/2000/09/xmldsig#sha1' />        <DigestValue>qKjVTKAe8rHZxLLFW9+ZVBdyRt8=</DigestValue>      </Reference>    </SignedInfo>    <SignatureValue>nLJdZQWuihKgwGV27ClGfA/J5IjiA4R960McZdnkWSHrqD2TXGSKI0NChLO1x2CNvHAWcV+66E1kmqhxSkk49G3KMG3jIsPYc+ca4S693CkBJPqvOur14FeJxegQ/0es1R1se/twnxpt/aOEQHvWzKRNVkvv35Pbty1CXQ1gWcU=</SignatureValue>    <KeyInfo>      <X509Data>        <X509Certificate>MIIFUzCCBDugAwIBAgIQSUJS8pELZyjasDkgGzKm0TANBgkqhkiG9w0BAQUFADBuMQswCQYDVQQGEwJCUjETMBEGA1UEChMKSUNQLUJyYXNpbDEsMCoGA1UECxMjU2VjcmV0YXJpYSBkYSBSZWNlaXRhIEZlZGVyYWwgLSBTUkYxHDAaBgNVBAMTE0FDIENlcnRpU2lnbiBTUkYgVjMwHhcNMDYwNzE5MDAwMDAwWhcNMDkwNzE4MjM1OTU5WjCB1DELMAkGA1UEBhMCQlIxEzARBgNVBAoUCklDUC1CcmFzaWwxKjAoBgNVBAsTIVNlY3JldGFyaWEgZGEgUmVjZWl0YSBGZWRlcmFsLVNSRjETMBEGA1UECxQKU1JGIGUtQ05QSjELMAkGA1UECBMCUkoxFzAVBgNVBAcUDlJJTyBERSBKQU5FSVJPMUkwRwYDVQQDE0BUSVBMQU4gQ09OU1VMVE9SSUEgRSBTRVJWSUNPUyBFTSBJTkZPUk1BVElDQSBMVERBOjA0NjQyNTU0MDAwMTQzMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCx86LAoJRVmtQMzmtdWpyNgKy200+bwjtz/TuywNcTjvfw7qHFGIgTjipmuZ3zhX28CgYLYXp3tj1Dfh2B7EhjHdLJPfvoF4MgbN/dQGXmGpMpF5cNxYusOGCZiyASvI7Gqt/xE4xLSIalNr6kF6CaPLkpFgTNNe+WQkG0fMqsQQIDAQABo4ICCDCCAgQwgbEGA1UdEQSBqTCBpqA/BgVgTAEDBKA2DDQyNDA3MTk3NjA3MTM4NTM3Nzg2MDAwMDAwMDAwMDAwMDAwMDAwOTI5OTA2MjFDTkggIFJKoB8GBWBMAQMCoBYMFEZFUk5BTkRPIFNJTFZBIEJSQUdBoBkGBWBMAQMDoBAMDjA0NjQyNTU0MDAwMTQzoBEGBWBMAQMHoAgMBjIzOTU0OIEUZmJyYWdhQHRpcGxhbi5jb20uYnIwCQYDVR0TBAIwADBiBgNVHR8EWzBZMFegVaBThlFodHRwOi8vaWNwLWJyYXNpbC5jZXJ0aXNpZ24uY29tLmJyL3JlcG9zaXRvcmlvL2xjci9BQ0NlcnRpU2lnblNSRlYzL0xhdGVzdENSTC5jcmwwHwYDVR0jBBgwFoAU9p1ZXf6/xXLN3c7ELmYbLu4Iz3YwDgYDVR0PAQH/BAQDAgXgMFUGA1UdIAROMEwwSgYGYEwBAgMGMEAwPgYIKwYBBQUHAgEWMmh0dHA6Ly9pY3AtYnJhc2lsLmNlcnRpc2lnbi5jb20uYnIvcmVwb3NpdG9yaW8vZHBjMB0GA1UdJQQWMBQGCCsGAQUFBwMEBggrBgEFBQcDAjA4BggrBgEFBQcBAQQsMCowKAYIKwYBBQUHMAGGHGh0dHA6Ly9vY3NwLmNlcnRpc2lnbi5jb20uYnIwDQYJKoZIhvcNAQEFBQADggEBAC5w/CBXAykvPSbBGf+u0UPcWVJATL2ix0hCfNUVtHaCjMz8hRjgYqmhpefzDm2LCTvoCPzG6XQBYxAmnDhX1f/gyjHz+E1xJg451qtqcyCJ9861o9R2bHd4zR0DuyxCNGOTiYJ4Gc/Xa4xqECorAx5ktkk1T/HOc1K/ntRGpdL+llsO/jqSRmTOnRgdeNHcKkyXsOgL5BwxxgGNuIyqirgGXW0by4Io1GnSXtixxfvEOnqOicxBY6AcVS9HHuhmOBYiK9skAUp0Sm2v41hpsC8uIkfUeRxsJIp2CNZ4DjoyfmKwNLMCRZQAKpwMXyyHZlX1a4o/9iGTszNeeShw61g=</X509Certificate>      </X509Data>    </KeyInfo>  </Signature></PedidoEnvioRPS>");

            XmlDocument XmlConsultaNCPJ = new XmlDocument();
            XmlConsultaNCPJ.LoadXml("<?xml version='1.0' encoding='UTF-8'?><p1:PedidoConsultaCNPJ xmlns:p1='http://www.prefeitura.sp.gov.br/nfe' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'>  <Cabecalho Versao='1'>    <CPFCNPJRemetente>      <CNPJ>04642554000143</CNPJ>    </CPFCNPJRemetente>  </Cabecalho>  <CNPJContribuinte>    <CNPJ>04642554000143</CNPJ>  </CNPJContribuinte>  <Signature xmlns='http://www.w3.org/2000/09/xmldsig#'>    <SignedInfo>      <CanonicalizationMethod Algorithm='http://www.w3.org/TR/2001/REC-xml-c14n-20010315' />      <SignatureMethod Algorithm='http://www.w3.org/2000/09/xmldsig#rsa-sha1' />      <Reference URI='x'>        <Transforms>          <Transform Algorithm='http://www.w3.org/2000/09/xmldsig#enveloped-signature' />          <Transform Algorithm='http://www.w3.org/TR/2001/REC-xml-c14n-20010315' />        </Transforms>       <DigestMethod Algorithm='http://www.w3.org/2000/09/xmldsig#sha1' />        <DigestValue>DTGUiqjTVe8mb9zmRSNqBrnLQv4=</DigestValue>      </Reference>    </SignedInfo>    <SignatureValue>hRXpqac0od8DFVFf9W9ygOByqnpa2Tg+crjQhCwRZmNhxdxmhuygjpsoAxWynrH9Hqe7bs9or8qrdwU29nQy9Ky1jAN9vdRStBees7hvhk9/0NH7BBv1sOk17BFVPUVqDrClHDhdW8xn0AriOAcGWl3yuQggU3Y9iMdqLJf+Veg=</SignatureValue>    <KeyInfo>      <X509Data>        <X509Certificate>MIIFUzCCBDugAwIBAgIQSUJS8pELZyjasDkgGzKm0TANBgkqhkiG9w0BAQUFADBuMQswCQYDVQQGEwJCUjETMBEGA1UEChMKSUNQLUJyYXNpbDEsMCoGA1UECxMjU2VjcmV0YXJpYSBkYSBSZWNlaXRhIEZlZGVyYWwgLSBTUkYxHDAaBgNVBAMTE0FDIENlcnRpU2lnbiBTUkYgVjMwHhcNMDYwNzE5MDAwMDAwWhcNMDkwNzE4MjM1OTU5WjCB1DELMAkGA1UEBhMCQlIxEzARBgNVBAoUCklDUC1CcmFzaWwxKjAoBgNVBAsTIVNlY3JldGFyaWEgZGEgUmVjZWl0YSBGZWRlcmFsLVNSRjETMBEGA1UECxQKU1JGIGUtQ05QSjELMAkGA1UECBMCUkoxFzAVBgNVBAcUDlJJTyBERSBKQU5FSVJPMUkwRwYDVQQDE0BUSVBMQU4gQ09OU1VMVE9SSUEgRSBTRVJWSUNPUyBFTSBJTkZPUk1BVElDQSBMVERBOjA0NjQyNTU0MDAwMTQzMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCx86LAoJRVmtQMzmtdWpyNgKy200+bwjtz/TuywNcTjvfw7qHFGIgTjipmuZ3zhX28CgYLYXp3tj1Dfh2B7EhjHdLJPfvoF4MgbN/dQGXmGpMpF5cNxYusOGCZiyASvI7Gqt/xE4xLSIalNr6kF6CaPLkpFgTNNe+WQkG0fMqsQQIDAQABo4ICCDCCAgQwgbEGA1UdEQSBqTCBpqA/BgVgTAEDBKA2DDQyNDA3MTk3NjA3MTM4NTM3Nzg2MDAwMDAwMDAwMDAwMDAwMDAwOTI5OTA2MjFDTkggIFJKoB8GBWBMAQMCoBYMFEZFUk5BTkRPIFNJTFZBIEJSQUdBoBkGBWBMAQMDoBAMDjA0NjQyNTU0MDAwMTQzoBEGBWBMAQMHoAgMBjIzOTU0OIEUZmJyYWdhQHRpcGxhbi5jb20uYnIwCQYDVR0TBAIwADBiBgNVHR8EWzBZMFegVaBThlFodHRwOi8vaWNwLWJyYXNpbC5jZXJ0aXNpZ24uY29tLmJyL3JlcG9zaXRvcmlvL2xjci9BQ0NlcnRpU2lnblNSRlYzL0xhdGVzdENSTC5jcmwwHwYDVR0jBBgwFoAU9p1ZXf6/xXLN3c7ELmYbLu4Iz3YwDgYDVR0PAQH/BAQDAgXgMFUGA1UdIAROMEwwSgYGYEwBAgMGMEAwPgYIKwYBBQUHAgEWMmh0dHA6Ly9pY3AtYnJhc2lsLmNlcnRpc2lnbi5jb20uYnIvcmVwb3NpdG9yaW8vZHBjMB0GA1UdJQQWMBQGCCsGAQUFBwMEBggrBgEFBQcDAjA4BggrBgEFBQcBAQQsMCowKAYIKwYBBQUHMAGGHGh0dHA6Ly9vY3NwLmNlcnRpc2lnbi5jb20uYnIwDQYJKoZIhvcNAQEFBQADggEBAC5w/CBXAykvPSbBGf+u0UPcWVJATL2ix0hCfNUVtHaCjMz8hRjgYqmhpefzDm2LCTvoCPzG6XQBYxAmnDhX1f/gyjHz+E1xJg451qtqcyCJ9861o9R2bHd4zR0DuyxCNGOTiYJ4Gc/Xa4xqECorAx5ktkk1T/HOc1K/ntRGpdL+llsO/jqSRmTOnRgdeNHcKkyXsOgL5BwxxgGNuIyqirgGXW0by4Io1GnSXtixxfvEOnqOicxBY6AcVS9HHuhmOBYiK9skAUp0Sm2v41hpsC8uIkfUeRxsJIp2CNZ4DjoyfmKwNLMCRZQAKpwMXyyHZlX1a4o/9iGTszNeeShw61g=</X509Certificate>      </X509Data>    </KeyInfo>  </Signature></p1:PedidoConsultaCNPJ>");
                                                                                                                                                              StringWriter sw = new StringWriter();
            XmlTextWriter tx = new XmlTextWriter(sw);
            doc.WriteTo(tx);

            X509Certificate2 xCert;
            xCert = new X509Certificate2(@"C:\#PROJETOS\#ContadorVirtual\#certificado digital\13382380_MARCO_ZERO_COMERCIO_DE_ROUPAS_E_SERVICOS_DE_APOIO70306022000160.p12", "cpe361");

            var assin = "31000000AAAAA00000000000220070110TNN00000000205000000000000050000002658100022333180803";
            var result = SignRPS(xCert, assin);

            var chavePublica = xCert.GetPublicKey();

            string str = sw.ToString();// 
            n.ClientCertificates.Add(Buscar_Certificado_Nome(""));

            //AssinaturaDigital();
            //var x = n.ConsultaNFeEmitidas(1, str);


            var consulta = n.ConsultaCNPJ(1, XmlConsultaNCPJ.InnerXml);

            var x = n.EnvioRPS(1, str);
            
            var xx= n.TesteEnvioLoteRPS(1,doc.InnerXml);

            





            //MessageBox.Show(n.EnvioRPS(1, ""));


        }

        private void XML()
        {
            XmlTextWriter writer = new XmlTextWriter(@"c:\dados\filmes.xml", null);
            
            //inicia o documento xml
            writer.WriteStartDocument();
            //escreve o elmento raiz
            writer.WriteStartElement("filmes");
            writer.WriteAttributeString("ano", "2017");
            //Escreve os sub-elementos
            writer.WriteElementString("titulo", "Cada & Companhia");
            writer.WriteElementString("titulo", "007 contra Godzila");
            writer.WriteElementString("titulo", "O segredo do Dr. Haus's");
            // encerra o elemento raiz
            writer.WriteEndElement();
            //Escreve o XML para o arquivo e fecha o objeto escritor
            writer.Close();
        }
        private void AssinaturaDigital()
        {
            XmlDocument docRequest = new XmlDocument();
            docRequest.PreserveWhitespace = false;

            docRequest.LoadXml("<?xml version='1.0' encoding='utf-8'?><p1:PedidoConsultaNFePeriodo xmlns:p1='http://www.prefeitura.sp.gov.br/nfe' xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance'><Cabecalho Versao='1'><CPFCNPJRemetente><CNPJ>70306022000160</CNPJ></CPFCNPJRemetente><CPFCNPJ><CNPJ>10970887000285</CNPJ></CPFCNPJ><Inscricao>31000000</Inscricao><dtInicio>2015-01-01</dtInicio><dtFim>2015-10-31</dtFim><NumeroPagina>1</NumeroPagina></Cabecalho><Signature xmlns='http://www.w3.org/2000/09/xmldsig#'><SignedInfo><CanonicalizationMethod Algorithm='http://www.w3.org/TR/2001/REC-xml-c14n-20010315' /><SignatureMethod Algorithm='http://www.w3.org/2000/09/xmldsig#rsa-sha1' /><Reference URI=''><Transforms><Transform Algorithm='http://www.w3.org/2000/09/xmldsig#enveloped-signature' /><Transform Algorithm='http://www.w3.org/TR/2001/REC-xml-c14n-20010315' /></Transforms><DigestMethod Algorithm='http://www.w3.org/2000/09/xmldsig#sha1' /><DigestValue>MLNPmnDg+TdeC9jdIp0ldJOVQGI=</DigestValue></Reference></SignedInfo><SignatureValue>VWCYdIxH+IXmlCuNcYy/Z8CN/fDWs/koDaezE2T942WY1YWJMG7p4OvUDR2uwZlUIwZxqKvtY9CRaa15kMlbyPiQ5gd95qUHObHp8gLEYQhOq7PFq9hL6dehVyxibbWlPMTpGp2EtHob8Xo7X81XwEVu1zJIMwaFc6N5F6A8xQQ=</SignatureValue><KeyInfo><X509Data><X509Certificate>MIIFUzCCBDugAwIBAgIQSUJS8pELZyjasDkgGzKm0TANBgkqhkiG9w0BAQUFADBuMQswCQYDVQQGEwJCUjETMBEGA1UEChMKSUNQLUJyYXNpbDEsMCoGA1UECxMjU2VjcmV0YXJpYSBkYSBSZWNlaXRhIEZlZGVyYWwgLSBTUkYxHDAaBgNVBAMTE0FDIENlcnRpU2lnbiBTUkYgVjMwHhcNMDYwNzE5MDAwMDAwWhcNMDkwNzE4MjM1OTU5WjCB1DELMAkGA1UEBhMCQlIxEzARBgNVBAoUCklDUC1CcmFzaWwxKjAoBgNVBAsTIVNlY3JldGFyaWEgZGEgUmVjZWl0YSBGZWRlcmFsLVNSRjETMBEGA1UECxQKU1JGIGUtQ05QSjELMAkGA1UECBMCUkoxFzAVBgNVBAcUDlJJTyBERSBKQU5FSVJPMUkwRwYDVQQDE0BUSVBMQU4gQ09OU1VMVE9SSUEgRSBTRVJWSUNPUyBFTSBJTkZPUk1BVElDQSBMVERBOjA0NjQyNTU0MDAwMTQzMIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCx86LAoJRVmtQMzmtdWpyNgKy200+bwjtz/TuywNcTjvfw7qHFGIgTjipmuZ3zhX28CgYLYXp3tj1Dfh2B7EhjHdLJPfvoF4MgbN/dQGXmGpMpF5cNxYusOGCZiyASvI7Gqt/xE4xLSIalNr6kF6CaPLkpFgTNNe+WQkG0fMqsQQIDAQABo4ICCDCCAgQwgbEGA1UdEQSBqTCBpqA/BgVgTAEDBKA2DDQyNDA3MTk3NjA3MTM4NTM3Nzg2MDAwMDAwMDAwMDAwMDAwMDAwOTI5OTA2MjFDTkggIFJKoB8GBWBMAQMCoBYMFEZFUk5BTkRPIFNJTFZBIEJSQUdBoBkGBWBMAQMDoBAMDjA0NjQyNTU0MDAwMTQzoBEGBWBMAQMHoAgMBjIzOTU0OIEUZmJyYWdhQHRpcGxhbi5jb20uYnIwCQYDVR0TBAIwADBiBgNVHR8EWzBZMFegVaBThlFodHRwOi8vaWNwLWJyYXNpbC5jZXJ0aXNpZ24uY29tLmJyL3JlcG9zaXRvcmlvL2xjci9BQ0NlcnRpU2lnblNSRlYzL0xhdGVzdENSTC5jcmwwHwYDVR0jBBgwFoAU9p1ZXf6/xXLN3c7ELmYbLu4Iz3YwDgYDVR0PAQH/BAQDAgXgMFUGA1UdIAROMEwwSgYGYEwBAgMGMEAwPgYIKwYBBQUHAgEWMmh0dHA6Ly9pY3AtYnJhc2lsLmNlcnRpc2lnbi5jb20uYnIvcmVwb3NpdG9yaW8vZHBjMB0GA1UdJQQWMBQGCCsGAQUFBwMEBggrBgEFBQcDAjA4BggrBgEFBQcBAQQsMCowKAYIKwYBBQUHMAGGHGh0dHA6Ly9vY3NwLmNlcnRpc2lnbi5jb20uYnIwDQYJKoZIhvcNAQEFBQADggEBAC5w/CBXAykvPSbBGf+u0UPcWVJATL2ix0hCfNUVtHaCjMz8hRjgYqmhpefzDm2LCTvoCPzG6XQBYxAmnDhX1f/gyjHz+E1xJg451qtqcyCJ9861o9R2bHd4zR0DuyxCNGOTiYJ4Gc/Xa4xqECorAx5ktkk1T/HOc1K/ntRGpdL+llsO/jqSRmTOnRgdeNHcKkyXsOgL5BwxxgGNuIyqirgGXW0by4Io1GnSXtixxfvEOnqOicxBY6AcVS9HHuhmOBYiK9skAUp0Sm2v41hpsC8uIkfUeRxsJIp2CNZ4DjoyfmKwNLMCRZQAKpwMXyyHZlX1a4o/9iGTszNeeShw61g=</X509Certificate></X509Data></KeyInfo></Signature></p1:PedidoConsultaNFePeriodo>");
            XmlNodeList ListInfNFe = docRequest.GetElementsByTagName("Signature");
            X509Certificate2 xCert;
            xCert = new X509Certificate2(@"C:\#PROJETOS\#ContadorVirtual\#certificado digital\13382380_MARCO_ZERO_COMERCIO_DE_ROUPAS_E_SERVICOS_DE_APOIO70306022000160.p12", "cpe361");

            foreach (XmlElement infNFe in ListInfNFe)

            {

                string id = infNFe.Attributes.GetNamedItem("Id").InnerText;
                var signedXml = new SignedXml(infNFe);
                signedXml.SigningKey = xCert.PrivateKey;

                Reference reference = new Reference("#" + id);
                reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
                reference.AddTransform(new XmlDsigC14NTransform());
                signedXml.AddReference(reference);

                KeyInfo keyInfo = new KeyInfo();
                keyInfo.AddClause(new KeyInfoX509Data(xCert));

                signedXml.KeyInfo = keyInfo;

                signedXml.ComputeSignature();

                XmlElement xmlSignature = docRequest.CreateElement("Signature", "http://www.w3.org/2000/09/xmldsig#");
                XmlElement xmlSignedInfo = signedXml.SignedInfo.GetXml();
                XmlElement xmlKeyInfo = signedXml.KeyInfo.GetXml();

                XmlElement xmlSignatureValue = docRequest.CreateElement("SignatureValue", xmlSignature.NamespaceURI);
                string signBase64 = Convert.ToBase64String(signedXml.Signature.SignatureValue);
                XmlText text = docRequest.CreateTextNode(signBase64);
                xmlSignatureValue.AppendChild(text);

                xmlSignature.AppendChild(docRequest.ImportNode(xmlSignedInfo, true));
                xmlSignature.AppendChild(xmlSignatureValue);
                xmlSignature.AppendChild(docRequest.ImportNode(xmlKeyInfo, true));

                var evento = docRequest.GetElementsByTagName("TAG_EXTERNA_QUE_CONTERA_A_ASSINATURA");
                evento[0].AppendChild(xmlSignature);

            }
        }

        private static bool ValidateWithCertificate(SignedXml signer)
        {
            var keyInfoEnumerator = signer.KeyInfo.GetEnumerator();

            while (keyInfoEnumerator.MoveNext())
            {
                var x509Data = keyInfoEnumerator.Current as KeyInfoX509Data;

                if (x509Data != null && x509Data.Certificates.Count > 0)
                    foreach (var cert in x509Data.Certificates)
                        if (signer.CheckSignature((X509Certificate2)cert, true))
                            return true;
            }

            return false;
        }


        public string SignRPS(X509Certificate2 cert, String sAssinatura)
        {

            //recebe o certificado e a string a ser assinada 
            System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();

            //pega a chave privada do certificado digital 
            rsa = cert.PrivateKey as RSACryptoServiceProvider;

            //cria o array de bytes e realiza a conversao da string em array de bytes 
            byte[] sAssinaturaByte = enc.GetBytes(sAssinatura);

            RSAPKCS1SignatureFormatter rsaf = new RSAPKCS1SignatureFormatter(rsa);
            SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider();

            //cria a variavel hash que armazena o resultado do sha1 
            byte[] hash;
            hash = sha1.ComputeHash(sAssinaturaByte);

            //definimos o metodo a ser utilizado na criptografia e assinamos 
            rsaf.SetHashAlgorithm("SHA1");
            sAssinaturaByte = rsaf.CreateSignature(hash);

            //por fim fazemos a conversao do array de bytes para string 
            string convertido;
            convertido = Convert.ToBase64String(sAssinaturaByte);

            return convertido;
        }
        private static void SetCertificatePolicy()
        {
            // Código necessário para acessar serviços remotos
            // Evita o erro HTTP 417
            ServicePointManager.Expect100Continue = false;

            ServicePointManager.ServerCertificateValidationCallback += RemoteCertificateValidate;
        }

        private static bool RemoteCertificateValidate(object sender, X509Certificate cert, X509Chain chain,
            SslPolicyErrors error)
        {
            return true;
        }

        public void teste()
        {
            var store = new X509Store(StoreLocation.CurrentUser);

            store.Open(OpenFlags.ReadOnly);

            var certificates = store.Certificates;
            foreach (var certificate in certificates)
            {
                var friendlyName = certificate.FriendlyName;
#pragma warning disable CS0618 // Type or member is obsolete
                var xname = certificate.GetName(); //obsolete
#pragma warning restore CS0618 // Type or member is obsolete
                Console.WriteLine(friendlyName);
            }

            store.Close();
        }

        // declarando variaveis e enumerador necessarios
        enum ResultadoAssinatura
        {
            XMLAssinadoSucesso,
            CertificadoDigitalInexistente,
            TagAssinaturaNaoExiste,
            TagAssinaturaNaoUnica,
            ErroAssinarDocumento,
            XMLMalFormado,
            ProblemaAcessoCertificadoDigital
        }

        private ResultadoAssinatura Resultado;
        private string Mensagem;

        public string AssinarXML(string pArquivoXML, string pUri, X509Certificate2 pCertificado)
        {
            StreamReader sr = File.OpenText(pArquivoXML);
            string XML = sr.ReadToEnd();
            sr.Close();

            // parametros de retorno
            string XMLAssinado = String.Empty;
            this.Resultado = ResultadoAssinatura.XMLAssinadoSucesso;
            this.Mensagem = "Assinatura realizada com sucesso.";

            try
            {
                // verificando existencia de certificado utilizado na assinatura
                string subject = String.Empty;
                if (pCertificado != null)
                    subject = pCertificado.Subject.ToString();

                X509Certificate2 x509Certificate = new X509Certificate2();
                X509Store store = new X509Store("MY", StoreLocation.CurrentUser);
                store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly);
                X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
                X509Certificate2Collection collection1 = (X509Certificate2Collection)collection.Find(X509FindType.FindBySubjectDistinguishedName, subject, false);

                if (collection1.Count == 0)
                {
                    this.Resultado = ResultadoAssinatura.CertificadoDigitalInexistente;
                    this.Mensagem = "Problemas no certificado digital.";
                }
                else
                {
                    XmlDocument documento = new XmlDocument();
                    documento.PreserveWhitespace = false;

                    try
                    {
                        // verificando elemento de referencia
                        documento.LoadXml(XML);
                        int qtdeRefUri = documento.GetElementsByTagName(pUri).Count;

                        if (qtdeRefUri == 0)
                        {
                            this.Resultado = ResultadoAssinatura.TagAssinaturaNaoExiste;
                            this.Mensagem = "A tag de assinatura " + pUri.Trim() + " não existe.";
                        }
                        else
                        {
                            if (qtdeRefUri > 1)
                            {
                                this.Resultado = ResultadoAssinatura.TagAssinaturaNaoUnica;
                                this.Mensagem = "A tag de assinatura " + pUri.Trim() + " não é unica.";
                            }
                            else
                            {
                                try
                                {
                                    // selecionando certificado digital baseado no subject
                                    x509Certificate = collection1[0];

                                    SignedXml docXML = new SignedXml(documento);
                                    docXML.SigningKey = x509Certificate.PrivateKey;

                                    Reference reference = new Reference();
                                    XmlAttributeCollection uri = documento.GetElementsByTagName(pUri).Item(0).Attributes;

                                    foreach (XmlAttribute atributo in uri)
                                    {
                                        if (atributo.Name == "Id")
                                            reference.Uri = "#" + atributo.InnerText;
                                    }

                                    // adicionando EnvelopedSignatureTransform a referencia
                                    XmlDsigEnvelopedSignatureTransform envelopedSigntature = new XmlDsigEnvelopedSignatureTransform();
                                    reference.AddTransform(envelopedSigntature);

                                    XmlDsigC14NTransform c14Transform = new XmlDsigC14NTransform();
                                    reference.AddTransform(c14Transform);

                                    docXML.AddReference(reference);

                                    // carrega o certificado em KeyInfoX509Data para adicionar a KeyInfo
                                    KeyInfo keyInfo = new KeyInfo();
                                    keyInfo.AddClause(new KeyInfoX509Data(x509Certificate));

                                    docXML.KeyInfo = keyInfo;
                                    docXML.ComputeSignature();

                                    // recuperando a representacao do XML assinado
                                    XmlElement xmlDigitalSignature = docXML.GetXml();

                                    documento.DocumentElement.AppendChild(documento.ImportNode(xmlDigitalSignature, true));

                                    XMLAssinado = documento.OuterXml;
                                }
                                catch (Exception caught)
                                {
                                    this.Resultado = ResultadoAssinatura.ErroAssinarDocumento;
                                    this.Mensagem = "Erro ao assinar o documento - " + caught.Message;
                                }
                            }
                        }
                    }
                    catch (Exception caught)
                    {
                        this.Resultado = ResultadoAssinatura.XMLMalFormado;
                        this.Mensagem = "XML mal formado - " + caught.Message;
                    }
                }
            }
            catch (Exception caught)
            {
                this.Resultado = ResultadoAssinatura.ProblemaAcessoCertificadoDigital;
                this.Mensagem = "Problema ao acessar o certificado digital - " + caught.Message;
            }

            return XMLAssinado;
        }
        public static byte[] SignFile(X509Certificate2Collection certs, byte[] data)
        {
            try
            {
                ContentInfo content = new ContentInfo(data);
                SignedCms signedCms = new SignedCms(content, false);
                //if (VerifySign(data))
                //{
                //    signedCms.Decode(data);
                //}
                foreach (X509Certificate2 cert in certs)
                {
                    CmsSigner signer = new CmsSigner(cert);
                    signer.IncludeOption = X509IncludeOption.WholeChain;
                    signedCms.ComputeSignature(signer);
                }
                return signedCms.Encode();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao assinar arquivo. A mensagem retornada foi: " + ex.Message);
            }
        }

        public static X509Certificate2 Buscar_Certificado_Nome(string _nm_certificado)
        {
         

            X509Certificate2 X509_certificado = new X509Certificate2();
            X509Store X509_store = new X509Store("MY", StoreLocation.CurrentUser);
            X509_store.Open(OpenFlags.ReadOnly | OpenFlags.OpenExistingOnly | OpenFlags.IncludeArchived);
            X509Certificate2Collection X509_collection0 = (X509Certificate2Collection)X509_store.Certificates;
            X509Certificate2Collection X509_collection1 = (X509Certificate2Collection)X509_collection0.Find(X509FindType.FindByTimeValid, DateTime.Now, false);
            X509Certificate2Collection X509_collection2 = (X509Certificate2Collection)X509_collection0.Find(X509FindType.FindByKeyUsage, X509KeyUsageFlags.DigitalSignature, false);
            SetCertificatePolicy();
            if (_nm_certificado == "")
            {
                X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(X509_collection2, "Certificado(s) Digital(is) disponível(is)", "Selecione o Certificado Digital para uso no aplicativo", X509SelectionFlag.SingleSelection);
                if (scollection.Count == 0)
                {
                    //Nenhum certificado escolhido
                    X509_certificado.Reset();
                }
                else
                {
                    X509_certificado = scollection[0];
                }
            }
            else
            {
                X509Certificate2Collection scollection = X509_collection1.Find(X509FindType.FindByThumbprint, "ce c9 ed 7a b6 54 fe fe 1b bf 31 78 ef 5f 74 be be 6e e8 12", true);
                if (scollection.Count == 0)
                {
                    //Nenhum certificado válido foi encontrado com o nome informado
                    X509_certificado.Reset();
                }
                else
                {
                    X509_certificado = scollection[0];
                }
            }
            X509_store.Close();

            CmsSigner signer = new CmsSigner(X509_certificado);
            signer.IncludeOption = X509IncludeOption.WholeChain;
           // signedCms.ComputeSignature(signer);


            return X509_certificado;
        }

        //public void Assinatura() { 
        //var xml = new XmlDocument();   // <---- aqui declaro um novo xml
        //xml.LoadXml(sbXml.ToString()); // <-- carrego o xml que quero assinar, no meu caso, dependendo do xml a montar, uso um StringBuilder doc pequenos

        //var i = 0;
        //        var docXML = new SignedXml(xml); // <-- instancia classe de criptografia

        //        docXML.SigningKey = NFe_Rec.ClientCredentials.ClientCertificate.Certificate.PrivateKey; // <--adiciono o certificado digital, instanciado o serviço que vou usar no caso "NFe_Rec", que ira  para autenticar a operação e atribuindo a docXML.SigningKey.

        //var refer = new Reference(); // <-- Adicionando Reference

        //        refer.Uri = "#ID" + (CASO O PADRÃO DO DOC FISCAL SEJA O MESMO, O CPF VEM AQUI); // <-- Essa é palavra chave
        //        refer.AddTransform(new XmlDsigEnvelopedSignatureTransform());
        //refer.AddTransform(new XmlDsigC14NTransform());
        //docXML.AddReference(refer); 

        //var ki = new KeyInfo();
        //ki.AddClause(new KeyInfoX509Data(NFe_Rec.ClientCredentials.ClientCertificate.Certificate));
        //docXML.KeyInfo = ki; // <-- refente a algumas computações e tags especificas do padrão X509 que são adicionados.

        //docXML.ComputeSignature(); // <-- calcula e assinatura,com base no arquivo xml lá em cima e Uri que informamos;
        //i++;

        //xml.ChildNodes[1].ChildNodes[i].AppendChild(xml.ImportNode(docXML.GetXml(), true)); // <-- aqui adiciona dentro do node que quiser a assinatura, nas NF(s), ficam dentro da tag <NFe> depois de todos os outros childs do node.
        //}


    }
}
