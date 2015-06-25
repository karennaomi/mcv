using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LM.Core.Domain
{
    public class PontoDemanda : IValidatableObject
    {
        public PontoDemanda()
        {
            DataInclusao = DateTime.Now;
            Ativo = true;
            QuantidadeDiasCoberturaEstoque = Constantes.PontoDemanda.QuantidadeDiasCoberturaEstoque;
        }
        public long Id { get; set; }
        public string Nome { get; set; }
        public short? QuantidadeDiasAlertaReposicao { get; set; }
        public short? QuantidadeDiasCoberturaEstoque { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public TipoPontoDemanda? Tipo { get; set; }
        public bool Ativo { get; set; }

        public virtual Usuario UsuarioCriador { get; set; }
        public virtual Endereco Endereco { get; set; }
        public virtual ICollection<GrupoDeIntegrantes> GruposDeIntegrantes { get; set; }
        public virtual ICollection<Lista> Listas { get; set; }
        public virtual ICollection<Loja> LojasFavoritas { get; set; }

        public IList<Integrante> IntegrantesAtivos()
        {
            return GruposDeIntegrantes.Where(g => g.Integrante.Ativo).Select(g => g.Integrante).ToList();
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (QuantidadeDiasAlertaReposicao.HasValue && !new[] {7, 14, 28}.Contains(QuantidadeDiasAlertaReposicao.Value))
            {
                yield return new ValidationResult("Frequência inválida.", new[] { "QuantidadeDiasAlertaReposicao" });
            }
        }
    }
}
