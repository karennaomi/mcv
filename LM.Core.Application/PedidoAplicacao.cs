using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface IPedidoAplicacao
    {
        IEnumerable<PedidoItem> ListarItensPorCategoria(long pontoDemandaId, int categoriaId);
        IEnumerable<PedidoItem> ListarItensPorStatus(long pontoDemandaId, StatusPedido status);
        IList<Categoria> ListarSecoes(long pontoDemandaId, StatusPedido status);
        void RemoverItem(long pontoDemandaId, long itemId);
        void AtualizarQuantidadeDoItem(long pontoDemandaId, long itemId, decimal quantidade);
        PedidoItem AdicionarItem(long pontoDemandaId, PedidoItem item);
    }

    public class PedidoAplicacao : IPedidoAplicacao
    {
        private readonly IRepositorioPedido _repositorio;
        public PedidoAplicacao(IRepositorioPedido repositorio)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<PedidoItem> ListarItensPorCategoria(long pontoDemandaId, int categoriaId)
        {
            return _repositorio.ListarItensPorCategoria(pontoDemandaId, categoriaId);
        }

        public IEnumerable<PedidoItem> ListarItensPorStatus(long pontoDemandaId, StatusPedido status)
        {
            return _repositorio.ListarItensPorStatus(pontoDemandaId, status);
        }

        public IList<Categoria> ListarSecoes(long pontoDemandaId, StatusPedido status)
        {
            return _repositorio.ListarSecoes(pontoDemandaId, status);
        }

        public void RemoverItem(long pontoDemandaId, long itemId)
        {
            _repositorio.RemoverItem(pontoDemandaId, itemId);
        }

        public void AtualizarQuantidadeDoItem(long pontoDemandaId, long itemId, decimal quantidade)
        {
            _repositorio.AtualizarQuantidadeDoItem(pontoDemandaId, itemId, quantidade);
        }

        public PedidoItem AdicionarItem(long pontoDemandaId, PedidoItem item)
        {
            return _repositorio.AdicionarItem(pontoDemandaId, item);
        }
    }
}
