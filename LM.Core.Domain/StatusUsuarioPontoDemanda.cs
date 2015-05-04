using System;

namespace LM.Core.Domain
{
    public class StatusUsuarioPontoDemanda
    {
        public StatusUsuarioPontoDemanda()
        {
            DataInclusao = DataAlteracao = DateTime.Now;
        }

        public long Id { get; set; }
        public virtual Usuario Usuario { get; set; }
        public long? PontoDemandaId { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public StatusCadastro StatusCadastro { get; set; }
    }
}
