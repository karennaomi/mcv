﻿using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class ListaEF : IRepositorioLista
    {
        private readonly ContextoEF _contexto;
        private readonly IRepositorioProcedures _repoLancamentoEstoque;
        public ListaEF(IRepositorioProcedures repoLancamentoEstoque)
        {
            _contexto = new ContextoEF();
            _repoLancamentoEstoque = repoLancamentoEstoque;
        }
        public ListaEF(ContextoEF contexto, IRepositorioProcedures repoLancamentoEstoque)
        {
            _contexto = contexto;
            _repoLancamentoEstoque = repoLancamentoEstoque;
        }

        public Lista ObterListaPorPontoDemanda(long pontoDemandaId)
        {
            return _contexto.Listas.Include("Itens.Periodo").FirstOrDefault(l => l.PontoDemanda.Id == pontoDemandaId);
        }

        public ListaItem AdicionarItem(Lista lista, ListaItem novoItem, long usuarioId)
        {
            return new ComandoCriarItemNaLista(_contexto, lista, novoItem).Executar(usuarioId);
        }

        public void AtualizarPeriodoDoItem(ListaItem item, int periodoId)
        {
            item.Periodo = _contexto.Set<Periodo>().Single(p => p.Id == periodoId);
        }

        public IEnumerable<ListaItem> BuscarItens(Lista lista, long pontoDemandaId, string termo)
        {
            var produtoEF = new ProdutoEF(_contexto);
            var produtosIds = produtoEF.Buscar(termo).Where(Produto.ProtectProductPredicate(pontoDemandaId)).Select(p => p.Id);
            return lista.Itens.Where(i => produtosIds.Contains(i.Produto.Id));
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }

        public void LancarEstoque(long pontoDemandaId, long integranteId, int? produtoId, decimal? quantidade)
        {
            _repoLancamentoEstoque.LancarEstoque(pontoDemandaId, 5, produtoId, quantidade, integranteId);
        }
    }
}
