using System;

namespace LM.Core.Domain
{
    public class PedidoItem
    {
        public PedidoItem()
        {
            DataInclusao = DataAlteracao = DateTime.Now;
            Status = StatusPedido.Pendente;
        }
        
        public long Id { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public decimal Quantidade { get; set; }
        public StatusPedido Status { get; set; }
        public DateTime Data { get; set; }

        public virtual Produto Produto { get; set; }
        public virtual PontoDemanda PontoDemanda { get; set; }
        public virtual Integrante Integrante { get; set; }
    }
}
