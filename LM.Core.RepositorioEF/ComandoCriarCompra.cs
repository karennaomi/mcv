using System;
using LM.Core.Domain;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class ComandoCriarCompra
    {
        private readonly ContextoEF _contexto;
        private Compra _novaCompra;
        private readonly ListaEF _listaRepo;
        public ComandoCriarCompra(ContextoEF contexto, Compra novaCompra)
        {
            _contexto = contexto;
            _listaRepo = new ListaEF(_contexto);
            _novaCompra = novaCompra;
        }

        public Compra Executar()
        {
            ChecarIntegrante();
            ChecarPontoDemanda();
            ChecarItens();
            _novaCompra = _contexto.Compras.Add(_novaCompra);

            return _novaCompra;
        }

        private void ChecarIntegrante()
        {
            _novaCompra.Integrante = _contexto.Integrantes.Single(i => i.Id == _novaCompra.Integrante.Id);
        }

        private void ChecarPontoDemanda()
        {
            _novaCompra.PontoDemanda = _contexto.PontosDemanda.Single(p => p.Id == _novaCompra.PontoDemanda.Id);
        }

        private void ChecarItens()
        {
            var lista = _listaRepo.ObterListaPorPontoDemanda(_novaCompra.PontoDemanda.Id);
            var listaItens = lista.Itens;
            foreach (var compraItem in _novaCompra.Itens)
            {
                if (compraItem is ListaCompraItem)
                {
                    var listaCompraItem = compraItem as ListaCompraItem;
                    listaCompraItem.Item = listaCompraItem.Item.Id == 0 ? CriarItemNaLista(listaCompraItem, lista) : listaCompraItem.Item = listaItens.Single(i => i.Id == listaCompraItem.Item.Id);
                }
                else if (compraItem is PedidoCompraItem)
                {
                    var pedidoCompraItem = compraItem as PedidoCompraItem;
                    pedidoCompraItem.Item = _contexto.PedidoItens.Single(p => p.Id == pedidoCompraItem.Item.Id);
                    if (pedidoCompraItem.Status == StatusCompra.Comprado)
                    {
                        CriarItemNaLista(pedidoCompraItem, lista);
                    }
                }
            }
        }

        private ListaItem CriarItemNaLista(CompraItem compraItem, Lista lista)
        {
            var listaItem = new ListaItem
            {
                Periodo = new Periodo {Id = 12 /* Eventual */},
                QuantidadeDeConsumo = compraItem.Quantidade,
                QuantidadeEmEstoque = compraItem.Quantidade
            };
            var listaCompraItem = compraItem as ListaCompraItem;
            if (listaCompraItem != null)
            {
                listaItem.Produto = listaCompraItem.Item.Produto;
            }
            else
            {
                var item = compraItem as PedidoCompraItem;
                if(item != null)
                {
                    listaItem.Produto = item.Item.Produto;
                }
                else
                {
                    throw new ApplicationException("Tipo de compra item inválido.");
                }
            }
            return _listaRepo.AdicionarItem(lista, listaItem, _novaCompra.Integrante.Usuario.Id);
        }
    }
}
