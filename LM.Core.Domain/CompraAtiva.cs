using System;

namespace LM.Core.Domain
{
    public class CompraAtiva
    {
        public CompraAtiva()
        {
            InicioCompra = DateTime.Now;
        }

        public int Id { get; set; }
        public DateTime InicioCompra { get; set; }
        public DateTime? FimCompra { get; set; }

        public virtual PontoDemanda PontoDemanda { get; set; }
        public virtual Usuario Usuario { get; set; }

        public bool CompraFinalizada()
        {
            return FimCompra.HasValue;
        }
    }
}
