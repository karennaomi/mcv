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
        private readonly IUnitOfWork<ContextoEF> _uniOfWork;
        public ListaEF(IUnitOfWork<ContextoEF> uniOfWork)
        {
            _uniOfWork = uniOfWork;
        }
        
        public ListaItem AdicionarItem(long pontoDemandaId, ListaItem item)
        {
            var lista = ObterListaPorPontoDemanda(pontoDemandaId);
            _uniOfWork.Contexto.Entry(item.Produto).State = EntityState.Unchanged;
            _uniOfWork.Contexto.Entry(item.Periodo).State = EntityState.Unchanged;
            item.DataInclusao = DateTime.Now;
            item.DataAlteracao = DateTime.Now;
            item.Status = "I";
            lista.Itens.Add(item);
            _uniOfWork.Contexto.SaveChanges();
            return item;
        }

        public void RemoverItem(long pontoDemandaId, long itemId)
        {
            var lista = ObterListaPorPontoDemanda(pontoDemandaId);
            var item = ObterItem(lista, itemId);
            item.DataAlteracao = DateTime.Now;
            lista.Itens.Remove(item);
            _uniOfWork.Contexto.Entry(item).State = EntityState.Deleted;
            _uniOfWork.Contexto.SaveChanges();
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
            _uniOfWork.Contexto.SaveChanges();
        }

        public void AtualizarConsumoDoItem(long pontoDemandaId, long itemId, decimal quantidade)
        {
            var item = ObterItem(ObterListaPorPontoDemanda(pontoDemandaId), itemId);
            item.QuantidadeDeConsumo = quantidade;
            item.DataAlteracao = DateTime.Now;
            _uniOfWork.Contexto.SaveChanges();
        }

        public void AtualizarPeriodoDoItem(long pontoDemandaId, long itemId, int periodoId)
        {
            var item = ObterItem(ObterListaPorPontoDemanda(pontoDemandaId), itemId);
            item.Periodo = new Periodo { Id = periodoId };
            item.DataAlteracao = DateTime.Now;
            _uniOfWork.Contexto.Entry(item).State = EntityState.Modified;
            _uniOfWork.Contexto.Entry(item.Periodo).State = EntityState.Unchanged;
            _uniOfWork.Contexto.SaveChanges();
        }

        private Lista ObterListaPorPontoDemanda(long pontoDemandaId)
        {
            var lista = _uniOfWork.Contexto.Listas.FirstOrDefault(l => l.PontoDemanda.Id == pontoDemandaId);
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
