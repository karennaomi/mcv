using System;

namespace LM.Core.Domain
{
    public class RecuperarSenha
    {
        public RecuperarSenha()
        {
            Token = Guid.NewGuid();
            DataInclusao = DateTime.Now;
        }

        public int Id { get; set; }
        public Guid Token { get; set; }
        public DateTime DataInclusao { get; set; }
        public virtual Usuario Usuario{ get; set; }
    }
}
