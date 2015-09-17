using System;

namespace LM.Core.Domain
{
    public enum AssinaturaStatus
    {
        Ativa, BloqueadaPagamentoRecusado, Expirada
    }

    public class Assinatura
    {
        public Assinatura()
        {
            DataInclusao = DataAlteracao = DateTime.Now;
        }

        public int Id { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual Plano Plano { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public AssinaturaStatus Status { get; set; }
    }
}
