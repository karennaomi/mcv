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

        public IEnumerable<PedidoItem> ListarItens(long pontoDemandaId)
        {
            return _contexto.PedidoItens.Where(p => p.PontoDemanda.Id == pontoDemandaId);
        }

        public PedidoItem AdicionarItem(long pontoDemandaId, PedidoItem item)
        {
            if (_contexto.PedidoItens.Any(i => i.PontoDemanda.Id == pontoDemandaId && i.Status == StatusPedido.Pendente && (i.Produto.Id == item.Produto.Id))) throw new ApplicationException("Este produto já existe na lista.");
            var itemDesativado = _contexto.PedidoItens.SingleOrDefault(i => i.PontoDemanda.Id == pontoDemandaId && i.Status == StatusPedido.ExcluidoPeloUsuario && (i.Produto.Id == item.Produto.Id));
            if (itemDesativado != null)
            {
                itemDesativado.Quantidade = item.Quantidade;
                itemDesativado.Status = StatusPedido.Pendente;
                itemDesativado.DataAlteracao = DateTime.Now;
                _contexto.SaveChanges();
                return itemDesativado;
            }

            item.PontoDemanda = _contexto.PontosDemanda.Single(p => p.Id == pontoDemandaId);
            item.Integrante = _contexto.Usuarios.Single(u => u.Id == item.Integrante.Usuario.Id).Integrante;
            item.Produto = ChecarProduto(item.Produto);
            _contexto.PedidoItens.Add(item);
            _contexto.SaveChanges();
            return item;
        }

        private Produto ChecarProduto(Produto produto)
        {
            if (produto.Id != 0) return _contexto.Produtos.Single(p => p.Id == produto.Id);

            var produtoRepo = new ProdutoEF(_contexto);
            return produtoRepo.Criar(produto);
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}
