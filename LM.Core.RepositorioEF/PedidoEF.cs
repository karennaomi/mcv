using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class PedidoEF : IRepositorioPedido
    {
        private readonly ContextoEF _contexto;
        public PedidoEF()
        {
            _contexto = new ContextoEF();
        }

        public IEnumerable<PedidoItem> ListarItensPorCategoria(long pontoDemandaId, int categoriaId)
        {
            var itens = ListarItens(pontoDemandaId);
            return itens.Where(i => i.Produto.Categorias.Any(c => c.CategoriaPai.Id == categoriaId) && i.Status != StatusPedido.ExcluidoPeloUsuario);
        }

        public IEnumerable<PedidoItem> ListarItensPorStatus(long pontoDemandaId, StatusPedido status)
        {
            var itens = ListarItens(pontoDemandaId);
            return itens.Where(i => i.Status == status);
        }

        public IList<Categoria> ListarSecoes(long pontoDemandaId, StatusPedido status)
        {
            var itens = ListarItens(pontoDemandaId).Where(i => i.Status == status);
            return itens.Select(i => i.Produto.Categorias.Select(c => c.CategoriaPai).First()).Distinct().OrderBy(c => c.Nome).ToList();
        }

        public void RemoverItem(long pontoDemandaId, long id)
        {
            var itens = ListarItens(pontoDemandaId);
            var item = ObterItem(itens, id);
            item.Status = StatusPedido.ExcluidoPeloUsuario;
            item.DataAlteracao = DateTime.Now;
            _contexto.SaveChanges();
        }

        public void AtualizarQuantidadeDoItem(long pontoDemandaId, long itemId, decimal quantidade)
        {
            var item = ObterItem(ListarItens(pontoDemandaId), itemId);
            item.Quantidade = quantidade;
            item.DataAlteracao = DateTime.Now;
            _contexto.SaveChanges();
        }

        public PedidoItem AdicionarItem(long pontoDemandaId, PedidoItem item)
        {
            if (_contexto.PedidoItens.Any(i => i.PontoDemanda.Id == pontoDemandaId && (i.Produto.Id == item.Produto.Id))) throw new ApplicationException("Este produto já existe na lista.");
            item.PontoDemanda = new PontoDemanda { Id = pontoDemandaId };
            item.Integrante = _contexto.Usuarios.Single(u => u.Id == item.Integrante.Usuario.Id).Integrante;
            _contexto.Entry(item.PontoDemanda).State = EntityState.Unchanged;
            _contexto.Entry(item.Produto).State = EntityState.Unchanged;
            item.Data = DateTime.Now;
            item.DataInclusao = DateTime.Now;
            item.DataAlteracao = DateTime.Now;
            item.Status = StatusPedido.Pendente;
            _contexto.PedidoItens.Add(item);
            _contexto.SaveChanges();
            return item;
        }

        private IEnumerable<PedidoItem> ListarItens(long pontoDemandaId)
        {
            return _contexto.PedidoItens.Where(p => p.PontoDemanda.Id == pontoDemandaId);
        }

        private static PedidoItem ObterItem(IEnumerable<PedidoItem> itens, long id)
        {
            var item = itens.SingleOrDefault(i => i.Id == id);
            if (item == null) throw new ApplicationException("O pedido não possui o item informado.");
            return item;
        }
    }
}
