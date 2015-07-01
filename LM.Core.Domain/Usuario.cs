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
        [LMRequired]
        public string Login { get; set; }

        [LMRequired]
        [LMMinLength(Constantes.Usuario.TamanhoMinimoSenha)]
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
            if (Integrante != null && Integrante.ObterIdade() < Constantes.Integrante.IdadeMinimaCadastro)
            {
                yield return new ValidationResult(string.Format(LMResource.Usuario_Validation_DataNascimento, Constantes.Integrante.IdadeMinimaCadastro), new[] { "Integrante.DataNascimento" });
            }
        }
    }
}

