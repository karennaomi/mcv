using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCV.Domain
{
    public class User
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public int DDD { get; set; }
        public int Telefone { get; set; }
        public string CPF { get; set; }
        public string Senha { get; set; }
        public DateTime DataInclusao { get; set; }
                                           //        public Endereco EnderecoResidencial { get; set; }
                                           //        public virtual ICollection<Empresas> IEmpresas { get; set; }

        public User()
        {
           // EnderecoResidencial = new Endereco();

         }
    }
}
