using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LM.Core.Domain;
using LM.Core.Repository.EntityFramework;

namespace LM.Core.Repository
{
    public interface IRepositorioLista
    {
        ListaItem AdicionarItem(long pontoDemandaId, ListaItem item);
        void RemoverItem(long pontoDemandaId, long itemId);
        IList<Categoria> ListarSecoes(long pontoDemandaId);
        IEnumerable<ListaItem> ListarItensPorCategoria(long pontoDemandaId, int categoriaId);
        void AtualizarEstoqueDoItem(long pontoDemandaId, long itemId, decimal quantidade);
        void AtualizarConsumoDoItem(long pontoDemandaId, long itemId, decimal quantidade);
        void AtualizarPeriodoDoItem(long pontoDemandaId, long itemId, int periodoId);
    }
    
    public class ListaEF : IRepositorioLista
    {
        private readonly ContextoEF _contextoEF;
        public ListaEF()
        {
            _contextoEF = new ContextoEF();
        }
        
        public ListaItem AdicionarItem(long pontoDemandaId, ListaItem item)
        {
            var lista = ObterListaPorPontoDemanda(pontoDemandaId);
            _contextoEF.Entry(item.Produto).State = EntityState.Unchanged;
            _contextoEF.Entry(item.Periodo).State = EntityState.Unchanged;
            item.DataInclusao = DateTime.Now;
            item.DataAlteracao = DateTime.Now;
            item.Status = "I";
            lista.Itens.Add(item);
            _contextoEF.SaveChanges();
            return item;
        }

        public void RemoverItem(long pontoDemandaId, long itemId)
        {
            var lista = ObterListaPorPontoDemanda(pontoDemandaId);
            var item = ObterItem(lista, itemId);
            item.DataAlteracao = DateTime.Now;
            lista.Itens.Remove(item);
            _contextoEF.Entry(item).State = EntityState.Deleted;
            _contextoEF.SaveChanges();
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
            _contextoEF.SaveChanges();
        }

        public void AtualizarConsumoDoItem(long pontoDemandaId, long itemId, decimal quantidade)
        {
            var item = ObterItem(ObterListaPorPontoDemanda(pontoDemandaId), itemId);
            item.QuantidadeDeConsumo = quantidade;
            item.DataAlteracao = DateTime.Now;
            _contextoEF.SaveChanges();
        }

        public void AtualizarPeriodoDoItem(long pontoDemandaId, long itemId, int periodoId)
        {
            var item = ObterItem(ObterListaPorPontoDemanda(pontoDemandaId), itemId);
            item.Periodo = new Periodo { Id = periodoId };
            item.DataAlteracao = DateTime.Now;
            _contextoEF.Entry(item).State = EntityState.Modified;
            _contextoEF.Entry(item.Periodo).State = EntityState.Unchanged;
            _contextoEF.SaveChanges();
        }

        private Lista ObterListaPorPontoDemanda(long pontoDemandaId)
        {
            var lista = _contextoEF.Listas.FirstOrDefault(l => l.PontoDemanda.Id == pontoDemandaId);
            if(lista == null) throw new ApplicationException("O ponto de demanda não possui uma lista.");
            return lista;
        }

        private static ListaItem ObterItem(Lista lista, long itemId)
        {
            var item = lista.Itens.SingleOrDefault(i => i.Id == itemId);
            if (lista == null) throw new ApplicationException("A lista não possui o item informado.");
            return item;
        }
    }
}
