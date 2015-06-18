using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LM.Core.Domain
{
    public class Usuario : IValidatableObject
    {
        public Usuario()
        {
            Ativo = true;
            DataInclusao = DateTime.Now;
        }

        public long Id { get; set; }
        [Required(ErrorMessage = "O campo Login é de preenchimento obrigatório!", AllowEmptyStrings = false)]
        public string Login { get; set; }
        [Required(ErrorMessage = "O campo Senha é de preenchimento obrigatório!", AllowEmptyStrings = false)]
        [MinLength(Constantes.Usuario.TamanhoMinimoSenha, ErrorMessage = "A senha deve possuir no mínimo " + Constantes.Usuario.TamanhoMinimoSenhaString + " caracteres.")]
        public string Senha { get; set; }
        public bool Ativo { get; set; }
        public string DeviceId { get; set; }
        public string DeviceType { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public virtual ICollection<StatusUsuarioPontoDemanda> StatusUsuarioPontoDemanda { get; set; }
        public virtual ICollection<ContratoUsuario> Contratos { get; set; }
        public virtual Integrante Integrante { get; set; }

        public StatusCadastro StatusAtual()
        {
            return StatusUsuarioPontoDemanda.OrderByDescending(s => s.Id).First().StatusCadastro;
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Integrante.ObterIdade() < Constantes.Integrante.IdadeMinimaCadastro)
            {
                yield return new ValidationResult(string.Format("O usuário deve ter {0} anos ou mais.", Constantes.Integrante.IdadeMinimaCadastro), new[] { "Integrante.DataNascimento" });
            }
        }
    }
}

