using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using LM.Core.Domain;
using LM.Core.Repository.EntityFramework;

namespace LM.Core.Repository
{
    public interface IRepositorioPedido
    {
        IEnumerable<PedidoItem> ListarItensPorCategoria(long pontoDemandaId, int categoriaId);
        IList<Categoria> ListarSecoes(long pontoDemandaId, StatusPedido status);
        void RemoverItem(long pontoDemandaId, long id);
        void AtualizarQuantidadeDoItem(long pontoDemandaId, long itemId, decimal quantidade);
        PedidoItem AdicionarItem(long pontoDemandaId, PedidoItem item);
    }

    public class PedidoEF : IRepositorioPedido
    {
        private readonly ContextoEF _contextoEF;
        public PedidoEF()
        {
            _contextoEF = new ContextoEF();
        }

        public IEnumerable<PedidoItem> ListarItensPorCategoria(long pontoDemandaId, int categoriaId)
        {
            var itens = ListarItens(pontoDemandaId);
            return itens.Where(i => i.Produto.Categorias.Any(c => c.CategoriaPai.Id == categoriaId) && i.Status != StatusPedido.ExcluidoPeloUsuario);
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
            _contextoEF.SaveChanges();
        }

        public void AtualizarQuantidadeDoItem(long pontoDemandaId, long itemId, decimal quantidade)
        {
            var item = ObterItem(ListarItens(pontoDemandaId), itemId);
            item.Quantidade = quantidade;
            item.DataAlteracao = DateTime.Now;
            _contextoEF.SaveChanges();
        }

        public PedidoItem AdicionarItem(long pontoDemandaId, PedidoItem item)
        {
            item.PontoDemanda = new PontoDemanda { Id = pontoDemandaId };
            var usuario = _contextoEF.Usuarios.Find(item.Integrante.Usuario.Id);
            item.Integrante = usuario.Integrante;
            _contextoEF.Entry(item.PontoDemanda).State = EntityState.Unchanged;
            _contextoEF.Entry(item.Produto).State = EntityState.Unchanged;
            item.Data = DateTime.Now;
            item.DataInclusao = DateTime.Now;
            item.DataAlteracao = DateTime.Now;
            item.Status = StatusPedido.Pendente;
            _contextoEF.PedidoItens.Add(item);
            _contextoEF.SaveChanges();
            return item;
        }

        private IEnumerable<PedidoItem> ListarItens(long pontoDemandaId)
        {
            return _contextoEF.PedidoItens.Where(p => p.PontoDemanda.Id == pontoDemandaId);
        }

        private PedidoItem ObterItem(IEnumerable<PedidoItem> itens, long id)
        {
            var item = itens.SingleOrDefault(i => i.Id == id);
            if (item == null) throw new ApplicationException("O pedido não possui o item informado.");
            return item;
        }
    }
}
