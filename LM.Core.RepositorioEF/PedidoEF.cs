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
            if (_contexto.PedidoItens.Any(i => i.PontoDemanda.Id == pontoDemandaId && (i.Produto.Id == item.Produto.Id))) throw new ApplicationException("Este produto já existe na lista.");
            item.PontoDemanda = new PontoDemanda { Id = pontoDemandaId };
            item.Integrante = _contexto.Usuarios.Single(u => u.Id == item.Integrante.Usuario.Id).Integrante;
            _contexto.Entry(item.PontoDemanda).State = EntityState.Unchanged;
            _contexto.Entry(item.Produto).State = EntityState.Unchanged;
            _contexto.PedidoItens.Add(item);
            _contexto.SaveChanges();
            return item;
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}
