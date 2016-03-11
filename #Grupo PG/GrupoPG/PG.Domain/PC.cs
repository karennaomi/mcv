using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Domain
{
    public class PC
    {
        public int Id { get; set; }
        public string ModeloEquipamento { get; set; }
        public string NomePC { get; set; }
        public virtual Endereco EnderecoPC { get; set; }

    }
}
