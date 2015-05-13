using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
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
            return _contexto.Listas.FirstOrDefault(l => l.PontoDemanda.Id == pontoDemandaId);
        }

        public ListaItem AdicionarItem(Lista lista, ListaItem novoItem)
        {
            return new ComandoCriarItemNaLista(_contexto, lista, novoItem).Executar();
        }

        public void RemoverItem(ListaItem item)
        {
            _contexto.Entry(item).State = EntityState.Deleted;
        }

        public void AtualizarPeriodoDoItem(ListaItem item)
        {
            _contexto.Entry(item).State = EntityState.Modified;
            _contexto.Entry(item.Periodo).State = EntityState.Unchanged;
        }

        public IEnumerable<ListaItem> BuscarItens(Lista lista, string termo)
        {
            var searchFts = FtsInterceptor.Fts(termo);
            return lista.Itens.Where(i => i.Produto.Info.Nome.Contains(searchFts));
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}
