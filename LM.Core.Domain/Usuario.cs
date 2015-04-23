using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace LM.Core.Domain
{
    public class Usuario : IValidatableObject
    {
        public long Id { get; set; }

        [Required(ErrorMessage = "O campo [Nome] é de preenchimento obrigatório!", AllowEmptyStrings = false)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo [Email] é de preenchimento obrigatório!", AllowEmptyStrings = false)]
        public string Email { get; set; }

        public string Login { get; set; }

        [Required(ErrorMessage = "O campo [Senha] é de preenchimento obrigatório!", AllowEmptyStrings = false)]
        [MinLength(6, ErrorMessage = "A senha deve possuir no mínimo 6 caracteres.")]
        public string Senha { get; set; }

        public string Cpf { get; set; }

        [DisplayName("Data de Nascimento")]
        [Required(ErrorMessage = "O campo [Data de Nascimento] é de preenchimento obrigatório!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime? DataNascimento { get; set; }

        [Required(ErrorMessage = "O campo [Sexo] é de preenchimento obrigatório!")]
        public string Sexo { get; set; }

        public string Origem { get; set; }
        public string ImgPerfil { get; set; }
        public TipoUsuario Tipo { get; set; }
        public string UsuarioIdRedeSocial { get; set; }

        public virtual ICollection<StatusUsuarioPontoDemanda> StatusUsuarioPontoDemanda { get; set; }
        public virtual ICollection<Integrante> MapIntegrantes { get; set; }

        public Integrante Integrante { get { return MapIntegrantes !=null ? MapIntegrantes.First() : null; } }

        public string DeviceId { get; set; }
        public string DeviceType { get; set; }

        public int ObterIdade()
        {
            if (!DataNascimento.HasValue) throw new ApplicationException("Data de nascimento inválida");
            var hoje = DateTime.Today;
            var idade = hoje.Year - DataNascimento.Value.Year;
            if (DataNascimento.Value.Date > hoje.Date.AddYears(-idade)) idade--;
            return idade;
        }

        public void DefinirSexo(string persona)
        {
            var tipo = persona.Split('-')[0];
            switch (tipo)
            {
                case "menino":
                    Sexo = "M";
                    break;
                case "menina":
                    Sexo = "F";
                    break;
                case "homem":
                    Sexo = "M";
                    break;
                case "mulher":
                    Sexo = "F";
                    break;
                case "empregado":
                    Sexo = "M";
                    break;
                case "empregada":
                    Sexo = "F";
                    break;
                case "idoso":
                    Sexo = "M";
                    break;
                case "idosa":
                    Sexo = "F";
                    break;
                default:
                    throw new ApplicationException("Tipo de usuário inválido");
            }
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var results = new List<ValidationResult>();
            if (Sexo.ToLower() != "m" && Sexo.ToLower() != "f")
            {
                results.Add(new ValidationResult("O sexo selecionado é inválido: " + Sexo, new[] { "Sexo" }));
            }
            return results;
        }

    }
}

