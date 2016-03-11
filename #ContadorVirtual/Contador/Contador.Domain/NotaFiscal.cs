using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contador.Domain
{
    public class NotaFiscal
    {
        public int Id { get; set; }
        public Empresa EmpresaEmissora { get; set; }
        public Empresa EmpresaTomadora { get; set; }
        public decimal VlNF { get; set; }


        public NotaFiscal()
        {
            EmpresaEmissora = new Empresa();
            EmpresaTomadora = new Empresa();
        }

    }
}
