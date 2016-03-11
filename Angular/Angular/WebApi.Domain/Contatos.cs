using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Domain
{
    public class Contatos
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public DateTime Data { get; set; }
        public string telefone { get; set; }
        public Operadora Operadora { get; set; }


        public Contatos()
        {
            Operadora = new Operadora();
        }
    }
}
