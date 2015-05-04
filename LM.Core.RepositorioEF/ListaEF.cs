using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class ListaEF : IRepositorioLista
    {
        private readonly ContextoEF _contexto;
        public ListaEF()
        {
            _contexto = new ContextoEF();
        }
        public ListaEF(ContextoEF contexto)
        {
            _contexto = contexto;
        }

        public Lista ObterListaPorPontoDemanda(long pontoDemandaId)
        {
            var lista = _contexto.Listas.FirstOrDefault(l => l.PontoDemanda.Id == pontoDemandaId);
            if (lista == null) throw new ApplicationException("O ponto de demanda não possui uma lista.");
            return lista;
        }

        public ListaItem AdicionarItem(long pontoDemandaId, ListaItem novoItem)
        {
            var lista = ObterListaPorPontoDemanda(pontoDemandaId);
            if(lista.Itens.Any(i => i.Produto.Id == novoItem.Produto.Id)) throw new ApplicationException("Este produto já existe na lista.");
            return new ComandoCriarItemNaLista(_contexto, lista, novoItem).Executar();
        }

        public void RemoverItem(long pontoDemandaId, long itemId)
        {
            var lista = ObterListaPorPontoDemanda(pontoDemandaId);
            var item = ObterItem(lista, itemId);
            item.DataAlteracao = DateTime.Now;
            lista.Itens.Remove(item);
            _contexto.Entry(item).State = EntityState.Deleted;
        }

        public IList<Categoria> ListarSecoes(long pontoDemandaId)
        {
            var lista = ObterListaPorPontoDemanda(pontoDemandaId);
            return lista.Itens.Select(i => i.Produto.Categorias.Select(c => c.CategoriaPai).First()).Distinct().OrderBy(c => c.Nome).ToList();
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
        }

        public void AtualizarConsumoDoItem(long pontoDemandaId, long itemId, decimal quantidade)
        {
            var item = ObterItem(ObterListaPorPontoDemanda(pontoDemandaId), itemId);
            item.QuantidadeDeConsumo = quantidade;
            item.DataAlteracao = DateTime.Now;
        }

        public void AtualizarPeriodoDoItem(long pontoDemandaId, long itemId, int periodoId)
        {
            var item = ObterItem(ObterListaPorPontoDemanda(pontoDemandaId), itemId);
            item.Periodo = new Periodo { Id = periodoId };
            item.DataAlteracao = DateTime.Now;
            _contexto.Entry(item).State = EntityState.Modified;
            _contexto.Entry(item.Periodo).State = EntityState.Unchanged;
        }

        private static ListaItem ObterItem(Lista lista, long itemId)
        {
            var item = lista.Itens.SingleOrDefault(i => i.Id == itemId);
            if (lista == null) throw new ApplicationException("A lista não possui o item informado.");
            return item;
        }

        public IEnumerable<ListaItem> BuscarItens(long pontoDemandaId, string termo)
        {
            var lista = ObterListaPorPontoDemanda(pontoDemandaId);
            var searchFts = FtsInterceptor.Fts(termo);
            return lista.Itens.Where(i => i.Produto.Info.Nome.Contains(searchFts));
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}
