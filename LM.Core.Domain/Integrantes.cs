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
        
        [Required(ErrorMessage = "O nome é de preenchimento obrigatório!", AllowEmptyStrings = false)]
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
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
                yield return new ValidationResult("O sexo é de preenchimento obrigatório!", new[] { "Sexo" });
            }
            else if (Sexo.ToLower() != "m" && Sexo.ToLower() != "f")
            {
                yield return new ValidationResult("O sexo selecionado é inválido: " + Sexo, new[] { "Sexo" });
            }

            if (!string.IsNullOrWhiteSpace(Email))
            {
                var regex = new Regex("^[A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,4}$", RegexOptions.IgnoreCase);
                if (!regex.IsMatch(Email))
                    yield return new ValidationResult("O e-mail informado é inválido: " + Email, new[] {"Email"});
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
            return Usuario == null && !string.IsNullOrWhiteSpace(Email) && ValidarDataConvite() && ObterIdade() >= Constantes.IdadeMinimaCadastro;
        }

        private bool ValidarDataConvite()
        {
            if (!DataConvite.HasValue) return true;
            return DataConvite.Value < DateTime.Now.AddDays(-Constantes.DiasMinimosParaConvidarNovamente);
        }
    }
}
