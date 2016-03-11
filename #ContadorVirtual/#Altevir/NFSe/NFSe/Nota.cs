using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NFSe
{
    class Nota
    {
        public enum Metodos
        {
            testeEnviarLoteRps,
            recepcionarLoteRps,
            consultarLoteRps,
            consultarNfse,
            cancelarNfse,
            envioRPS,
            consultaCNPJ,
            consultarPeriodo
            

        };

        public string numeroLote { get; set; }
        public string cnpjPrestador { get; set; }
        public string IMPrestador { get; set; }
        public string cnaePrestador { get; set; }
        public string dataVencimento { get; set; }
        public string numeroNota { get; set; }
        public string serieNota { get; set; }
        public string quantidadeRps { get; set; }
        public string tipoRps { get; set; }
        public string dataEmissao { get; set; }
        public string naturezaOperacao { get; set; }
        public string optanteSimples { get; set; }
        public string regimeEspecialTributacao { get; set; }
        public string incentivadorCulturalFiscal { get; set; }
        public string status { get; set; }
        public string valorNota { get; set; }
        public string deduzISS { get; set; }
        public string deduzPIS { get; set; }
        public string deduzCOFINS { get; set; }
        public string deduzCSLL { get; set; }
        public string deduzINSS { get; set; }
        public string deduzIR { get; set; }
        public string valorISS { get; set; }
        public string valorPIS { get; set; }
        public string valorCOFINS { get; set; }
        public string valorCSLL { get; set; }
        public string valorINSS { get; set; }
        public string valorIR { get; set; }
        public string aliquotaISS { get; set; }
        public string valorLiquidoNota { get; set; }
        public string itemListaServico { get; set; }
        public string codigoTributacaoMunicipio { get; set; }
        public string discriminacaoServico { get; set; }
        public string cpfCnpjTomador { get; set; }
        public string IMTomador { get; set; }
        public string razaoSocialTomador { get; set; }
        public string endereco { get; set; }
        public string numeroEndereco { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string codigoIBGE { get; set; }
        public string estado { get; set; }
        public string cep { get; set; }
        public string telefoneTomador { get; set; }
        public string emailTomador { get; set; }
    }
}
