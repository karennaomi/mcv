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
        IList<Categoria> ListarSecoes(long pontoDemandaId);
        IEnumerable<ListaItem> ListarItens(long pontoDemandaId);
        IEnumerable<ListaItem> ListarItensPorSecao(long pontoDemandaId, int secaoId);
        ListaItem AdicionarItem(long usuarioId, long pontoDemandaId, ListaItem item);
        void DesativarItem(long usuarioId, long pontoDemandaId, long itemId);
        void AtualizarItem(long usuarioId, long pontoDemandaId, ListaItem item);
        void AtualizarConsumoDoItem(long usuarioId, long pontoDemandaId, long itemId, decimal quantidade);
        void AtualizarEstoqueDoItem(long usuarioId, long pontoDemandaId, long itemId, decimal quantidade);
        void AtualizarPeriodoDoItem(long usuarioId, long pontoDemandaId, long itemId, int periodoId);
        void AtualizarEhEssencialDoItem(long usuarioId, long pontoDemandaId, long itemId, bool ehEssencial);
        IEnumerable<ListaItem> BuscarItens(long pontoDemandaId, string termo);
        IEnumerable<Periodo> PeriodosDeConsumo();
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

        public IEnumerable<ListaItem> ListarItens(long pontoDemandaId)
        {
            var lista = ObterListaPorPontoDemanda(pontoDemandaId);
            return lista.Itens.SomenteAtivos().QueNaoSaoSubstitutos().OrdenadoPorSecao().OrdenadoPorNomeDoProduto();
        }

        public IList<Categoria> ListarSecoes(long pontoDemandaId)
        {
            return ListarItens(pontoDemandaId).ListarSecoes();
        }

        public IEnumerable<ListaItem> ListarItensPorSecao(long pontoDemandaId, int secaoId)
        {
            return ListarItens(pontoDemandaId).DaSecao(secaoId).OrdenadoPorNomeDoProduto();
        }

        public ListaItem AdicionarItem(long usuarioId, long pontoDemandaId, ListaItem item)
        {
            var lista = ObterListaPorPontoDemanda(pontoDemandaId);
            if (lista.JaExisteProdutoNaLista(item)) throw new ApplicationException("Este produto já existe na lista.");
            item = _repositorio.AdicionarItem(lista, item, usuarioId);
            _repositorio.LancarEstoque(pontoDemandaId, usuarioId, item.Produto.Id, item.QuantidadeEstoque);
            _repositorio.RecalcularSugestao(pontoDemandaId, item.Produto.Id);
            return item;
        }

        public void DesativarItem(long usuarioId, long pontoDemandaId, long itemId)
        {
            var item = ObterItem(pontoDemandaId, itemId);
            item.DataAlteracao = DateTime.Now;
            item.Status = "I";
            _repositorio.Salvar();
        }

        public void AtualizarItem(long usuarioId, long pontoDemandaId, ListaItem item)
        {
            var itemToUpdate = ObterItem(pontoDemandaId, item.Id);
            itemToUpdate.QuantidadeConsumo = item.QuantidadeConsumo;
            itemToUpdate.QuantidadeEstoque = item.QuantidadeEstoque;
            itemToUpdate.EhEssencial = item.EhEssencial;
            itemToUpdate.DataAlteracao = DateTime.Now;
            _repositorio.AtualizarItem(itemToUpdate, item.Periodo.Id, usuarioId);
            _repositorio.LancarEstoque(pontoDemandaId, usuarioId, itemToUpdate.Produto.Id, itemToUpdate.QuantidadeEstoque);
            _repositorio.Salvar();
            _repositorio.RecalcularSugestao(pontoDemandaId, itemToUpdate.Produto.Id);
        }

        public void AtualizarConsumoDoItem(long usuarioId, long pontoDemandaId, long itemId, decimal quantidade)
        {
            var itemToUpdate = ObterItem(pontoDemandaId, itemId);
            itemToUpdate.QuantidadeConsumo = quantidade;
            itemToUpdate.DataAlteracao = DateTime.Now;
            _repositorio.AtualizarItem(itemToUpdate, itemToUpdate.Periodo.Id, usuarioId);
            _repositorio.Salvar();
            _repositorio.RecalcularSugestao(pontoDemandaId, itemToUpdate.Produto.Id);
        }

        public void AtualizarEstoqueDoItem(long usuarioId, long pontoDemandaId, long itemId, decimal quantidade)
        {
            var itemToUpdate = ObterItem(pontoDemandaId, itemId);
            itemToUpdate.QuantidadeEstoque = quantidade;
            itemToUpdate.DataAlteracao = DateTime.Now;
            _repositorio.AtualizarItem(itemToUpdate, itemToUpdate.Periodo.Id, usuarioId);
            _repositorio.LancarEstoque(pontoDemandaId, usuarioId, itemToUpdate.Produto.Id, quantidade);
            _repositorio.Salvar();
            _repositorio.RecalcularSugestao(pontoDemandaId, itemToUpdate.Produto.Id);
        }

        public void AtualizarPeriodoDoItem(long usuarioId, long pontoDemandaId, long itemId, int periodoId)
        {
            var itemToUpdate = ObterItem(pontoDemandaId, itemId);
            if (itemToUpdate.Periodo.Id == periodoId) return;
            itemToUpdate.DataAlteracao = DateTime.Now;
            _repositorio.AtualizarItem(itemToUpdate, periodoId, usuarioId);
            _repositorio.Salvar();
        }

        public void AtualizarEhEssencialDoItem(long usuarioId, long pontoDemandaId, long itemId, bool ehEssencial)
        {
            var itemToUpdate = ObterItem(pontoDemandaId, itemId);
            itemToUpdate.EhEssencial = ehEssencial;
            itemToUpdate.DataAlteracao = DateTime.Now;
            _repositorio.AtualizarItem(itemToUpdate, itemToUpdate.Periodo.Id, usuarioId);
            _repositorio.Salvar();
        }

        public IEnumerable<ListaItem> BuscarItens(long pontoDemandaId, string termo)
        {
            var lista = ObterListaPorPontoDemanda(pontoDemandaId);
            return _repositorio.BuscarItens(lista, pontoDemandaId, termo);
        }

        private ListaItem ObterItem(long pontoDemandaId, long itemId)
        {
            var item = ListarItens(pontoDemandaId).SingleOrDefault(i => i.Id == itemId);
            if (item == null) throw new ApplicationException("A lista não possui o item informado.");
            return item;
        }

        public IEnumerable<Periodo> PeriodosDeConsumo()
        {
            return _repositorio.PeriodosDeConsumo();
        }
    }
}
