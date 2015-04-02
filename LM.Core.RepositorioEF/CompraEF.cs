using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Data.Entity;

namespace LM.Core.RepositorioEF
{
    public class CompraEF : IRepositorioCompra
    {
        private readonly ContextoEF _contexto;
        public CompraEF()
        {
            _contexto = new ContextoEF();
        }

        public Compra Criar(Compra compra)
        {
            compra = _contexto.Compras.Add(compra);
            _contexto.Entry(compra.Integrante).State = EntityState.Unchanged;
            _contexto.Entry(compra.PontoDemanda).State = EntityState.Unchanged;
            foreach (var compraItem in compra.Itens)
            {
                if (compraItem is ListaCompraItem)
                {
                    var listaCompraItem = compraItem as ListaCompraItem;
                    _contexto.Entry(listaCompraItem.Item).State = EntityState.Unchanged;
                }
                else if(compraItem is PedidoCompraItem)
                {
                    var pedidoCompraItem = compraItem as PedidoCompraItem;
                    _contexto.Entry(pedidoCompraItem.Item).State = EntityState.Unchanged;
                }
                
            }
            _contexto.SaveChanges();
            return compra;
        }
    }
}
