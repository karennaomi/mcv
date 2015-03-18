using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LM.Core.Domain
{
    public class Cidade
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        [Required(ErrorMessage = "O campo [Estado] é de preenchimento obrigatório!", AllowEmptyStrings = false)]
        [DisplayName("Estado")]
        public virtual Uf Uf { get; set; }
    }
}
