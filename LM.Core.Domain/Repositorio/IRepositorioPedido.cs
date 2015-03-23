using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioPedido
    {
        IEnumerable<PedidoItem> ListarItensPorCategoria(long pontoDemandaId, int categoriaId);
        IEnumerable<PedidoItem> ListarItensPorStatus(long pontoDemandaId, StatusPedido status);
        IList<Categoria> ListarSecoes(long pontoDemandaId, StatusPedido status);
        void RemoverItem(long pontoDemandaId, long id);
        void AtualizarQuantidadeDoItem(long pontoDemandaId, long itemId, decimal quantidade);
        PedidoItem AdicionarItem(long pontoDemandaId, PedidoItem item);
    }
}
