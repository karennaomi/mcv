using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCV.Domain
{
    public class Movimentacao
    {
        public int idMovimentacao { get; set; }
        public int IdEmpresa { get; set; }
        public int IdTipoMovimentaco { get; set; }
        public float VlMovimentacao { get; set; }
        public DateTime DtMovimentacao { get; set; }
        public DateTime DtInclusao { get; set; }
    }
}
