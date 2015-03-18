using System;

namespace LM.Core.Domain
{
    public class ListaItem
    {
        public long Id { get; set; }
        public decimal? QuantidadeDeConsumo { get; set; }
        public decimal? QuantidadeEmEstoque { get; set; }
        public decimal? QuantidadeDoEstoqueEstimado { get; set; }
        public decimal? QuantidadeDeSugestaoDeCompra { get; set; }
        public string Status { get; set; }
        public decimal? ValorMedioDeConsumoPorIntegrante { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public virtual Lista Lista { get; set; }
        public virtual Produto Produto { get; set; }
        public virtual Periodo Periodo { get; set; }
    }
}
