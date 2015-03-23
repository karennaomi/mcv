using System;

namespace LM.Core.Domain
{
    public class CompraItem
    {
        public long Id { get; set; }
        public StatusCompra Status { get; set; }
        public DateTime? DataCompra { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? Valor { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        
        public virtual Compra Compra { get; set; }
        public virtual ListaItem ListaItem { get; set; }
        public virtual Produto Produto { get; set; }
    }
}
