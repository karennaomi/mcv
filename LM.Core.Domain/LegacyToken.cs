using System;

namespace LM.Core.Domain
{
    public class LegacyToken
    {
        public LegacyToken()
        {
            Id = Guid.NewGuid();
            DataInicio = DateTime.Now;
            DataValidade = DataInicio.AddDays(1);
        }
        public Guid Id { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataValidade { get; set; }
        public long UsuarioId { get; set; }
    }
}
