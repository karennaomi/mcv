using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LM.Core.Domain
{
    public enum IntegrantePapel : long
    {
        Administrador = 1,
        Colaborador = 2
    }

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
            DataInclusao = DataAlteracao = DateTime.Now;
            EhUsuarioSistema = true;
            Papel = IntegrantePapel.Administrador;
            Tipo = TipoIntegrante.Familia;
        }

        public long Id { get; set; }
        
        [Required(ErrorMessage = "O campo [Nome] é de preenchimento obrigatório!", AllowEmptyStrings = false)]
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cpf { get; set; }
        [DisplayName("Data de Nascimento")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataNascimento { get; set; }
        public bool EhUsuarioSistema { get; set; }
        public bool EhUsuarioConvidado { get; set; }
        public DateTime? DataConvite { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public bool Ativo { get; set; }
        public string Telefone { get; set; }
        public int? DDD { get; set; }
        public string Sexo { get; set; }
        public IntegrantePapel Papel { get; set; }
        public TipoIntegrante Tipo { get; set; }

        public virtual Usuario Usuario { get; set; }
        public virtual GrupoDeIntegrantes GrupoDeIntegrantes { get; set; }

        public int ObterIdade()
        {
            return LMHelper.ObterIdade(DataNascimento);
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Tipo == TipoIntegrante.Pet) yield break;
            
            if (string.IsNullOrWhiteSpace(Email))
            {
                yield return new ValidationResult("O campo [Email] é de preenchimento obrigatório!", new[] { "Email" });
            }

            if (!DataNascimento.HasValue)
            {
                yield return new ValidationResult("O campo [Data de Nascimento] é de preenchimento obrigatório!", new[] { "DataNascimento" });
            }

            if (string.IsNullOrWhiteSpace(Sexo))
            {
                yield return new ValidationResult("O campo [Sexo] é de preenchimento obrigatório!", new[] { "Sexo" });
            }
            else if (Sexo.ToLower() != "m" && Sexo.ToLower() != "f")
            {
                yield return new ValidationResult("O sexo selecionado é inválido: " + Sexo, new[] { "Sexo" });
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
        }

        public bool PodeSerConvidado()
        {
            return !string.IsNullOrWhiteSpace(Email) && ValidarDataConvite() && ObterIdade() >= 13;
        }

        private bool ValidarDataConvite()
        {
            if (!DataConvite.HasValue) return true;
            return DataConvite.Value < DateTime.Now.AddDays(-1);
        }
    }
}
