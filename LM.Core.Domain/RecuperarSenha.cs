using System;

namespace LM.Core.Domain
{
    public class RecuperarSenha
    {
        public const int ValidadeToken = 30; //minutos

        public RecuperarSenha()
        {
            Token = Guid.NewGuid();
            DataInclusao = DateTime.Now;
        }

        public int Id { get; set; }
        public Guid Token { get; set; }
        public DateTime DataInclusao { get; set; }
        public virtual Usuario Usuario{ get; set; }

        public DateTime DataExpiracao { get { return DataInclusao.AddMinutes(ValidadeToken); } }

        public bool TokenValido()
        {
            return DataExpiracao >= DateTime.Now;
        }
    }
}
