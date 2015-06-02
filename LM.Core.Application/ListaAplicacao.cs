﻿using System;
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
        void DesativarItem(long pontoDemandaId, long itemId);
        IList<Categoria> ListarSecoes(long pontoDemandaId);
        IEnumerable<ListaItem> ListarItens(long pontoDemandaId);
        IEnumerable<ListaItem> ListarItensPorCategoria(long pontoDemandaId, int categoriaId);
        void AtualizarItem(long pontoDemandaId, long itemId, decimal consumo, decimal estoque, int periodoId, bool ehEssencial);
        void AtualizarConsumoDoItem(long pontoDemandaId, long itemId, decimal quantidade);
        void AtualizarEstoqueDoItem(long pontoDemandaId, long itemId, decimal quantidade);
        void AtualizarPeriodoDoItem(long pontoDemandaId, long itemId, int periodoId);
        void AtualizarEhEssencialDoItem(long pontoDemandaId, long itemId, bool ehEssencial);
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
            if (lista.Itens.Any(i => i.Produto.Id == item.Produto.Id && i.Status == "A")) throw new ApplicationException("Este produto já existe na lista.");
            _repositorio.AdicionarItem(lista, item);
            return item;
        }

        public void DesativarItem(long pontoDemandaId, long itemId)
        {
            var item = ObterItem(pontoDemandaId, itemId);
            item.DataAlteracao = DateTime.Now;
            item.Status = "I";
            _repositorio.Salvar();
        }

        public IEnumerable<ListaItem> ListarItens(long pontoDemandaId)
        {
            var lista = ObterListaPorPontoDemanda(pontoDemandaId);
            return lista.Itens.Where(i => i.Status == "A");
        }

        public IList<Categoria> ListarSecoes(long pontoDemandaId)
        {
            return ListarItens(pontoDemandaId).ListarSecoes();
        }

        public IEnumerable<ListaItem> ListarItensPorCategoria(long pontoDemandaId, int categoriaId)
        {
            return ListarItens(pontoDemandaId).Where(i => i.Produto.Categorias.Any(c => c.CategoriaPai.Id == categoriaId));
        }

        public void AtualizarItem(long pontoDemandaId, long itemId, decimal consumo, decimal estoque, int periodoId, bool ehEssencial)
        {
            var item = ObterItem(pontoDemandaId, itemId);
            item.QuantidadeDeConsumo = consumo;
            item.QuantidadeEmEstoque = estoque;
            if(item.Periodo.Id != periodoId) item.Periodo = new Periodo { Id = periodoId };
            item.EhEssencial = ehEssencial;
            _repositorio.AtualizarPeriodoDoItem(item);
            item.DataAlteracao = DateTime.Now;
            _repositorio.Salvar();
        }

        public void AtualizarConsumoDoItem(long pontoDemandaId, long itemId, decimal quantidade)
        {
            var item = ObterItem(pontoDemandaId, itemId);
            item.QuantidadeDeConsumo = quantidade;
            item.DataAlteracao = DateTime.Now;
            _repositorio.Salvar();
        }

        public void AtualizarEstoqueDoItem(long pontoDemandaId, long itemId, decimal quantidade)
        {
            var item = ObterItem(pontoDemandaId, itemId);
            item.QuantidadeEmEstoque = quantidade;
            item.DataAlteracao = DateTime.Now;
            _repositorio.Salvar();
        }

        public void AtualizarPeriodoDoItem(long pontoDemandaId, long itemId, int periodoId)
        {
            var item = ObterItem(pontoDemandaId, itemId);
            if (item.Periodo.Id == periodoId) return;
            item.Periodo = new Periodo { Id = periodoId };
            item.DataAlteracao = DateTime.Now;
            _repositorio.AtualizarPeriodoDoItem(item);
            _repositorio.Salvar();
        }

        public void AtualizarEhEssencialDoItem(long pontoDemandaId, long itemId, bool ehEssencial)
        {
            var item = ObterItem(pontoDemandaId, itemId);
            item.EhEssencial = ehEssencial;
            item.DataAlteracao = DateTime.Now;
            _repositorio.Salvar();
        }

        public IEnumerable<ListaItem> BuscarItens(long pontoDemandaId, string termo)
        {
            var lista = ObterListaPorPontoDemanda(pontoDemandaId);
            return _repositorio.BuscarItens(lista, termo);
        }

        private ListaItem ObterItem(long pontoDemandaId, long itemId)
        {
            var item = ListarItens(pontoDemandaId).SingleOrDefault(i => i.Id == itemId);
            if (item == null) throw new ApplicationException("A lista não possui o item informado.");
            return item;
        }
    }
}
