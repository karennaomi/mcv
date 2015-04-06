using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface IListaAplicacao
    {
        ListaItem AdicionarItem(long pontoDemandaId, ListaItem item);
        void RemoverItem(long pontoDemandaId, long itemId);
        IList<Categoria> ListarSecoes(long pontoDemandaId);
        IEnumerable<ListaItem> ListarItensPorCategoria(long pontoDemandaId, int categoriaId);
        void AtualizarEstoqueDoItem(long pontoDemandaId, long itemId, decimal quantidade);
        void AtualizarConsumoDoItem(long pontoDemandaId, long itemId, decimal quantidade);
        void AtualizarPeriodoDoItem(long pontoDemandaId, long itemId, int periodoId);
    }

    public class ListaAplicacao : IListaAplicacao
    {
        private readonly IRepositorioLista _repositorio;
        private readonly IProdutoAplicacao _appProduto;
        public ListaAplicacao(IRepositorioLista repositorio, IProdutoAplicacao appProduto)
        {
            _repositorio = repositorio;
            _appProduto = appProduto;
        }

        public ListaItem AdicionarItem(long pontoDemandaId, ListaItem item)
        {
            if (item.Produto.Id == 0) _appProduto.Criar(item.Produto);
            return _repositorio.AdicionarItem(pontoDemandaId, item);
        }

        public void RemoverItem(long pontoDemandaId, long itemId)
        {
            _repositorio.RemoverItem(pontoDemandaId, itemId);
        }

        public IList<Categoria> ListarSecoes(long pontoDemandaId)
        {
            return _repositorio.ListarSecoes(pontoDemandaId);
        }

        public IEnumerable<ListaItem> ListarItensPorCategoria(long pontoDemandaId, int categoriaId)
        {
            return _repositorio.ListarItensPorCategoria(pontoDemandaId, categoriaId);
        }

        public void AtualizarEstoqueDoItem(long pontoDemandaId, long itemId, decimal quantidade)
        {
            _repositorio.AtualizarEstoqueDoItem(pontoDemandaId, itemId, quantidade);
        }

        public void AtualizarConsumoDoItem(long pontoDemandaId, long itemId, decimal quantidade)
        {
            _repositorio.AtualizarConsumoDoItem(pontoDemandaId, itemId, quantidade);
        }

        public void AtualizarPeriodoDoItem(long pontoDemandaId, long itemId, int periodoId)
        {
            _repositorio.AtualizarPeriodoDoItem(pontoDemandaId, itemId, periodoId);
        }
    }
}
