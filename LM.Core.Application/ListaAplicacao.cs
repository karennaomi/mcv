using System;
using System.Linq;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface IListaAplicacao
    {
        Lista ObterListaPorPontoDemanda(long pontoDemandaId);
        ListaItem AdicionarItem(long pontoDemandaId, ListaItem item);
        void RemoverItem(long pontoDemandaId, long itemId);
        IList<Categoria> ListarSecoes(long pontoDemandaId);
        IEnumerable<ListaItem> ListarItens(long pontoDemandaId);
        IEnumerable<ListaItem> ListarItensPorCategoria(long pontoDemandaId, int categoriaId);
        void AtualizarEstoqueDoItem(long pontoDemandaId, long itemId, decimal quantidade);
        void AtualizarConsumoDoItem(long pontoDemandaId, long itemId, decimal quantidade);
        void AtualizarPeriodoDoItem(long pontoDemandaId, long itemId, int periodoId);
        IEnumerable<ListaItem> BuscarItens(long pontoDemandaId, string termo);
    }

    public class ListaAplicacao : IListaAplicacao
    {
        private readonly IRepositorioLista _repositorio;
        public ListaAplicacao(IRepositorioLista repositorio)
        {
            _repositorio = repositorio;
        }

        public Lista ObterListaPorPontoDemanda(long pontoDemandaId)
        {
            var lista = _repositorio.ObterListaPorPontoDemanda(pontoDemandaId);
            if (lista == null) throw new ApplicationException("O ponto de demanda não possui uma lista.");
            return lista;
        }

        public ListaItem AdicionarItem(long pontoDemandaId, ListaItem item)
        {
            var lista = ObterListaPorPontoDemanda(pontoDemandaId);
            if (lista.Itens.Any(i => i.Produto.Id == item.Produto.Id)) throw new ApplicationException("Este produto já existe na lista.");
            _repositorio.AdicionarItem(lista, item);
            return item;
        }

        public void RemoverItem(long pontoDemandaId, long itemId)
        {
            var lista = ObterListaPorPontoDemanda(pontoDemandaId);
            var item = ObterItem(lista, itemId);
            item.DataAlteracao = DateTime.Now;
            lista.Itens.Remove(item);
            _repositorio.RemoverItem(item);
            _repositorio.Salvar();
        }

        public IList<Categoria> ListarSecoes(long pontoDemandaId)
        {
            var lista = ObterListaPorPontoDemanda(pontoDemandaId);
            return lista.Itens.Select(i => i.Produto.Categorias.Select(c => c.CategoriaPai).First()).Distinct().OrderBy(c => c.Nome).ToList();
        }

        public IEnumerable<ListaItem> ListarItens(long pontoDemandaId)
        {
            var lista = ObterListaPorPontoDemanda(pontoDemandaId);
            return lista.Itens;
        }

        public IEnumerable<ListaItem> ListarItensPorCategoria(long pontoDemandaId, int categoriaId)
        {
            var lista = ObterListaPorPontoDemanda(pontoDemandaId);
            return lista.Itens.Where(i => i.Produto.Categorias.Any(c => c.CategoriaPai.Id == categoriaId));
        }

        public void AtualizarEstoqueDoItem(long pontoDemandaId, long itemId, decimal quantidade)
        {
            var item = ObterItem(ObterListaPorPontoDemanda(pontoDemandaId), itemId);
            item.QuantidadeEmEstoque = quantidade;
            item.DataAlteracao = DateTime.Now;
            _repositorio.Salvar();
        }

        public void AtualizarConsumoDoItem(long pontoDemandaId, long itemId, decimal quantidade)
        {
            var item = ObterItem(ObterListaPorPontoDemanda(pontoDemandaId), itemId);
            item.QuantidadeDeConsumo = quantidade;
            item.DataAlteracao = DateTime.Now;
            _repositorio.Salvar();
        }

        public void AtualizarPeriodoDoItem(long pontoDemandaId, long itemId, int periodoId)
        {
            var item = ObterItem(ObterListaPorPontoDemanda(pontoDemandaId), itemId);
            item.Periodo = new Periodo { Id = periodoId };
            item.DataAlteracao = DateTime.Now;
            _repositorio.AtualizarPeriodoDoItem(item);
            _repositorio.Salvar();
        }

        public IEnumerable<ListaItem> BuscarItens(long pontoDemandaId, string termo)
        {
            var lista = ObterListaPorPontoDemanda(pontoDemandaId);
            return _repositorio.BuscarItens(lista, termo);
        }

        private static ListaItem ObterItem(Lista lista, long itemId)
        {
            var item = lista.Itens.SingleOrDefault(i => i.Id == itemId);
            if (lista == null) throw new ApplicationException("A lista não possui o item informado.");
            return item;
        }
    }
}
