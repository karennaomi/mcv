using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Domain
{
    public class Faturamento
    {
        public int Id { get; set; }
        public virtual OrdemServico OS { get; set; }
        public bool Enviado { get; set; }
        public bool Pago { get; set; }
        public DateTime DtEnvioPrevia { get; set; }
        public DateTime DtVencimento { get; set; }
        public DateTime DtPagamento { get; set; }
        public string NumeroNF { get; set; }

    }
}
