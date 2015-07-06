using System;
using System.ComponentModel.DataAnnotations;

namespace LM.Core.Domain
{
    public class PedidoItem : IItem, IValidatableObject
    {
        public PedidoItem()
        {
            DataInclusao = Data = DateTime.Now;
            Status = StatusPedido.Pendente;
        }
        
        public long Id { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public virtual Produto Produto { get; set; }

        public decimal Quantidade { get; set; }
        public StatusPedido Status { get; set; }
        public DateTime Data { get; set; }
        public virtual PontoDemanda PontoDemanda { get; set; }
        public virtual Integrante Integrante { get; set; }


        public PontoDemanda ObterPontoDemanda()
        {
            return PontoDemanda;
        }

        public decimal ObterQuantidadeParaCompra()
        {
            return Quantidade;
        }

        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Quantidade <= 0) yield return new ValidationResult("Quantidade deve ser maior que zero.", new[] { "Quantidade" });
        }
    }
}
