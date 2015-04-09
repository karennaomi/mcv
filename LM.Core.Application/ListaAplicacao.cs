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
        public ListaAplicacao(IRepositorioLista repositorio)
        {
            _repositorio = repositorio;
        }

        public ListaItem AdicionarItem(long pontoDemandaId, ListaItem item)
        {
            item = _repositorio.AdicionarItem(pontoDemandaId, item);
            _repositorio.Salvar();
            return item;
        }

        public void RemoverItem(long pontoDemandaId, long itemId)
        {
            _repositorio.RemoverItem(pontoDemandaId, itemId);
            _repositorio.Salvar();
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
            _repositorio.Salvar();
        }

        public void AtualizarConsumoDoItem(long pontoDemandaId, long itemId, decimal quantidade)
        {
            _repositorio.AtualizarConsumoDoItem(pontoDemandaId, itemId, quantidade);
            _repositorio.Salvar();
        }

        public void AtualizarPeriodoDoItem(long pontoDemandaId, long itemId, int periodoId)
        {
            _repositorio.AtualizarPeriodoDoItem(pontoDemandaId, itemId, periodoId);
            _repositorio.Salvar();
        }
    }
}
