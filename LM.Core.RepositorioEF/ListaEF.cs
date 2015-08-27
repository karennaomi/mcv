using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class ListaEF : IRepositorioLista
    {
        private readonly ContextoEF _contexto;
        private readonly IRepositorioProcedures _repoProcedures;
        public ListaEF(IRepositorioProcedures repoProcedures)
        {
            _contexto = new ContextoEF();
            _repoProcedures = repoProcedures;
        }
        public ListaEF(ContextoEF contexto, IRepositorioProcedures repoLancamentoEstoque)
        {
            _contexto = contexto;
            _repoProcedures = repoLancamentoEstoque;
        }

        public Lista ObterListaPorPontoDemanda(long pontoDemandaId)
        {
            return _contexto.Listas.Include("Itens.Periodo").FirstOrDefault(l => l.PontoDemanda.Id == pontoDemandaId);
        }

        public ListaItem AdicionarItem(Lista lista, ListaItem novoItem, long usuarioId)
        {
            return new ComandoCriarItemNaLista(_contexto, lista, novoItem).Executar(usuarioId);
        }

        public void AtualizarItem(ListaItem item, int periodoId, long usuarioId)
        {
            item.Periodo = _contexto.Set<Periodo>().Single(p => p.Id == periodoId);
            item.AtualizadoPor = _contexto.Usuarios.Find(usuarioId);
        }

        public IEnumerable<ListaItem> BuscarItens(Lista lista, long pontoDemandaId, string termo)
        {
            var produtoEF = new ProdutoEF(_contexto);
            var produtosIds = produtoEF.Buscar(termo).Where(Produto.ProtectProductPredicate(pontoDemandaId)).Select(p => p.Id);
            return lista.Itens.Where(i => produtosIds.Contains(i.Produto.Id) && i.Produto.Ativo == true);
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }

        public void LancarEstoque(long pontoDemandaId, long usuarioId, int? produtoId, decimal? quantidade)
        {
            var usuario = _contexto.Usuarios.Find(usuarioId);
            _repoProcedures.LancarEstoque(pontoDemandaId, 5, produtoId, quantidade, usuario.Integrante.Id);
        }

        public IEnumerable<Periodo> PeriodosDeConsumo()
        {
            return _contexto.Set<Periodo>();
        }

        public void RecalcularSugestao(long pontoDemandaId, int produtoId)
        {
            _repoProcedures.RecalcularSugestao(pontoDemandaId, produtoId);
        }
    }
}
