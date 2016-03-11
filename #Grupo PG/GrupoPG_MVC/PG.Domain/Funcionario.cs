using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Domain
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string NomeFuncionario { get; set; }
        public DateTime DtEntrada { get; set; }
        public DateTime DtSaida { get; set; }
    }
}
