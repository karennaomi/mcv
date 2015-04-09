using System;

namespace LM.Core.Domain
{
    public abstract class CompraItem
    {
        protected CompraItem()
        {
            var agora = DateTime.Now;
            DataCompra = agora;
            DataInclusao = agora;
            DataAlteracao = agora;
        }

        public long Id { get; set; }
        public int? ProdutoId { get; set; }
        public StatusCompra Status { get; set; }
        public DateTime? DataCompra { get; set; }
        public decimal? Quantidade { get; set; }
        public decimal? Valor { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public virtual Compra Compra { get; set; }
        public virtual CompraItemSubstituto ItemSubstituto { get; set; }
    }

    public class ListaCompraItem : CompraItem
    {
        public virtual ListaItem Item { get; set; }
    }

    public class PedidoCompraItem : CompraItem
    {
        public virtual PedidoItem Item { get; set; }
    }

    public class CompraItemSubstituto
    {
        public CompraItemSubstituto()
        {
            DataInclusao = DateTime.Now;
        }

        public long SubstitutoId { get; set; }
        public virtual CompraItem Original { get; set; }
        public string Motivo { get; set; }
        public DateTime? DataInclusao { get; set; }
    }
}
