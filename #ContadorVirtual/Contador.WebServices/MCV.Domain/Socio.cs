using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCV.Domain
{
    public class Socio
    {
        public int IdEmpresa { get; set; }
        public int IdSocio { get; set; }
        public string NomeSocio { get; set; }
        public int NrPercentual { get; set; }
        public DateTime DtInclusao { get; set; }
    }
}
