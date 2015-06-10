using System;

namespace LM.Core.Domain
{
    public class ContratoUsuario
    {
        public long Id { get; set; }
        public int ContratoId { get; set; }
        public int? PontoDemandaId { get; set; }

        public virtual Usuario Usuario { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
