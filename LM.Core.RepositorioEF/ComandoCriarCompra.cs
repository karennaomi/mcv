using System.Data.Entity;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using LM.Core.Domain;

namespace LM.Core.RepositorioEF
{
    public class ComandoCriarCompra
    {
        private readonly ContextoEF _contexto;
        private Compra _novaCompra;
        public ComandoCriarCompra(ContextoEF contexto, Compra novaCompra)
        {
            _contexto = contexto;
            _novaCompra = novaCompra;
        }

        public Compra Executar()
        {
            ChecarItensNovos();
            ChecarItens();
            
            _contexto.Entry(_novaCompra.Integrante).State = EntityState.Unchanged;
            ChecarPontoDemanda();
            
            _novaCompra = _contexto.Compras.Add(_novaCompra);
            return _novaCompra;
        }

        private void ChecarPontoDemanda()
        {
            var pontoDemandaLocal = _contexto.PontosDemanda.Local.SingleOrDefault(p => p.Id == _novaCompra.PontoDemanda.Id);
            if (pontoDemandaLocal != null)
            {
                _novaCompra.PontoDemanda = pontoDemandaLocal;
            }
            else
            {
                _contexto.Entry(_novaCompra.PontoDemanda).State = EntityState.Unchanged;
            }
        }

        private void ChecarItensNovos()
        {
            var listaRepo = new ListaEF(_contexto);
            var compraItensNovos = _novaCompra.Itens.OfType<ListaCompraItem>().Where(i => i.Item.Id == 0).ToList();
            foreach (var compraItemNovo in compraItensNovos)
            {
                compraItemNovo.Item.Periodo = new Periodo {Id = 12 /* Eventual */};
                compraItemNovo.Item.QuantidadeDeConsumo = compraItemNovo.Quantidade;
                compraItemNovo.Item.QuantidadeEmEstoque = compraItemNovo.Quantidade;
                compraItemNovo.Item.Status = "A";
                compraItemNovo.Item = listaRepo.AdicionarItem(_novaCompra.PontoDemanda.Id, compraItemNovo.Item);
            }
        }

        private void ChecarItens()
        {
            var listaLocal = _contexto.Listas.Local.SingleOrDefault(l => l.PontoDemanda.Id == _novaCompra.PontoDemanda.Id);
            foreach (var compraItem in _novaCompra.Itens)
            {
                if (compraItem is ListaCompraItem)
                {
                    var listaCompraItem = compraItem as ListaCompraItem;
                    if (listaLocal != null && listaCompraItem.Item.Id > 0)
                    {
                        listaCompraItem.Item = listaLocal.Itens.Single(i => i.Id == listaCompraItem.Item.Id);
                    }
                    else if(listaCompraItem.Item.Id > 0)
                    {
                        _contexto.Entry(listaCompraItem.Item).State = EntityState.Unchanged;
                    }
                }
                else if (compraItem is PedidoCompraItem)
                {
                    var pedidoCompraItem = compraItem as PedidoCompraItem;
                    if (pedidoCompraItem.Item.Id > 0) _contexto.Entry(pedidoCompraItem.Item).State = EntityState.Unchanged;
                }
            }
        }
    }
}
