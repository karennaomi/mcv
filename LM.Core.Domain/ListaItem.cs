
using System;
using System.ComponentModel.DataAnnotations;

namespace LM.Core.Domain
{
    public class ListaItem : IItem, IValidatableObject
    {
        public ListaItem()
        {
            DataInclusao = DateTime.Now;
            Status = "A";
        }

        public long Id { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public virtual Produto Produto { get; set; }

        public decimal? QuantidadeConsumo { get; set; }
        public decimal? QuantidadeEstoque { get; set; }
        public decimal? QuantidadeDoEstoqueEstimado { get; set; }
        public decimal? QuantidadeDeSugestaoDeCompra { get; set; }
        public string Status { get; set; }
        public decimal? ValorMedioDeConsumoPorIntegrante { get; set; }
        public bool EhSugestaoDeCompra { get; set; }
        public bool EhEssencial { get; set; }
        public virtual Lista Lista { get; set; }
        public virtual Periodo Periodo { get; set; }

        public PontoDemanda ObterPontoDemanda()
        {
            return Lista.PontoDemanda;
        }

        public decimal ObterQuantidadeParaCompra()
        {
            return QuantidadeDeSugestaoDeCompra ?? 0;
        }

        public System.Collections.Generic.IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!QuantidadeConsumo.HasValue || QuantidadeConsumo <= 0) yield return new ValidationResult("Quantidade consumida deve ser maior que zero.", new[] { "QuantidadeConsumo" });
            if (!QuantidadeEstoque.HasValue || QuantidadeEstoque < 0) yield return new ValidationResult("Quantidade em estoque deve ser maior ou igual a zero.", new[] { "QuantidadeEstoque" });
            if (Periodo == null) yield return new ValidationResult("O período de consumo deve ser informado.", new[] { "Periodo" });
        }
    }
}
