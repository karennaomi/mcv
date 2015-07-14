using System;
using System.ComponentModel.DataAnnotations;

namespace LM.Core.Domain
{
    public abstract class CompraItem
    {
        protected CompraItem()
        {
            var agora = DateTime.Now;
            DataCompra = agora;
            DataInclusao = agora;
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

    public class ListaCompraItem : CompraItem, IValidatableObject
    {
        public virtual ListaItem Item { get; set; }

        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Status == StatusCompra.Comprado && Quantidade == 0)
            {
                yield return new ValidationResult(string.Format("Atenção! Não se esqueça de preencher a quantidade do produto {0} que comprou.", Item.Produto.Nome()), new[] { "Quantidade" });
            }
        }
    }

    public class PedidoCompraItem : CompraItem, IValidatableObject
    {
        public virtual PedidoItem Item { get; set; }

        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Status == StatusCompra.Comprado && Quantidade == 0)
            {
                yield return new ValidationResult(string.Format("Atenção! Não se esqueça de preencher a quantidade do produto {0} que comprou.", Item.Produto.Nome()), new[] { "Quantidade" });
            }
        }
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
