using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace LM.Core.Domain
{
    public enum TipoIntegrante
    {
        NaoReconhecido = 0,
        Familia = 1,
        Empregado = 2,
        Pet = 3
    }

    public class Integrante : IValidatableObject
    {
        public Integrante()
        {
            Ativo = true;
            DataInclusao = DateTime.Now;
            Tipo = TipoIntegrante.Familia;
        }

        public long Id { get; set; }
        
        [LMRequired]
        [LMMaxLength(Constantes.Integrante.TamanhoMaximoNome)]
        public string Nome { get; set; }

        [LMRequired]
        [LMMaxLength(Constantes.Integrante.TamanhoMaximoEmail)]
        [LMEmailAttribute]
        public string Email { get; set; }

        public string Cpf { get; set; }
        public DateTime? DataNascimento { get; set; }
        public bool EhUsuarioConvidado { get; set; }
        public DateTime? DataConvite { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public string Telefone { get; set; }
        public string Sexo { get; set; }
        public TipoIntegrante Tipo { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual ICollection<GrupoDeIntegrantes> GruposDeIntegrantes { get; set; }

        public int ObterIdade()
        {
            return LMHelper.ObterIdade(DataNascimento);
        }

        public bool EhUsuarioDoSistema()
        {
            return Usuario != null;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Tipo == TipoIntegrante.Pet) yield break;
            
            if (string.IsNullOrWhiteSpace(Sexo))
            {
                yield return new ValidationResult(string.Format(LMResource.Default_Validation_Required, "Sexo"), new[] { "Sexo" });
            }
            else if (!Sexo.Equals(Constantes.Integrante.SexoMasculino, StringComparison.InvariantCultureIgnoreCase) && !Sexo.Equals(Constantes.Integrante.SexoFeminino, StringComparison.InvariantCultureIgnoreCase))
            {
                yield return new ValidationResult(LMResource.DefaultValidation_Selected, new[] { "Sexo" });
            }

            if (!string.IsNullOrWhiteSpace(Email))
            {
                var regex = new Regex(Constantes.RegexTemplates.EmailRegex, RegexOptions.IgnoreCase);
                if (!regex.IsMatch(Email))
                    yield return new ValidationResult(string.Format(LMResource.Default_Validation_RegularExpression, "Email"), new[] {"Email"});
            }
        }

        public void Atualizar(Integrante integrante)
        {
            if (Usuario != null)
            {
                Usuario.Login = integrante.Email;
            }
            Email = integrante.Email;
            Nome = integrante.Nome;
            DataNascimento = integrante.DataNascimento;
            Sexo = integrante.Sexo;
            Telefone = integrante.Telefone;
            Cpf = integrante.Cpf;
        }

        public bool PodeSerConvidado()
        {
            return Usuario == null && !string.IsNullOrWhiteSpace(Email) && ValidarDataConvite() && ObterIdade() >= Constantes.Integrante.IdadeMinimaCadastro;
        }

        private bool ValidarDataConvite()
        {
            if (!DataConvite.HasValue) return true;
            return DataConvite.Value < DateTime.Now.AddDays(-Constantes.Integrante.DiasMinimosParaConvidarNovamente);
        }
    }
}
