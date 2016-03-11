using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contador.Domain
{
    public   class User
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        //public Endereco EnderecoUsuario { get; set; }

        //public User()
        //{
        //    EnderecoUsuario = new Endereco();
        //}
    }
}
