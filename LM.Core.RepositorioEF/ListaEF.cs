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
            return _contexto.Listas.Include("Itens.Periodo").FirstOrDefault(l => l.PontoDemanda.Id == pontoDemandaId);
        }

        public ListaItem AdicionarItem(Lista lista, ListaItem novoItem)
        {
            return new ComandoCriarItemNaLista(_contexto, lista, novoItem).Executar();
        }

        public void AtualizarPeriodoDoItem(ListaItem item, int periodoId)
        {
            item.Periodo = _contexto.Set<Periodo>().Single(p => p.Id == periodoId);
        }

        public IEnumerable<ListaItem> BuscarItens(Lista lista, long usuarioId, long pontoDemandaId, string termo)
        {
            var produtoEF = new ProdutoEF(_contexto);
            var produtosIds = produtoEF.Buscar(usuarioId, pontoDemandaId, termo).Select(p => p.Id);
            return lista.Itens.Where(i => produtosIds.Contains(i.Produto.Id));
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }

        public void LancarEstoque(long pontoDemandaId, long integranteId, int? produtoId, decimal? quantidade)
        {
            var lancamentoEstoque = new LancamentoEstoque(_contexto);
            lancamentoEstoque.Lancar(pontoDemandaId, 5, produtoId, quantidade, integranteId);
        }
    }
}
