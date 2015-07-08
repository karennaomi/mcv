using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioPedido
    {
        IEnumerable<PedidoItem> ListarItens(long pontoDemandaId);
        PedidoItem AdicionarItem(long pontoDemandaId, PedidoItem item);
        void Salvar(bool skipValidation = false);
    }
}
