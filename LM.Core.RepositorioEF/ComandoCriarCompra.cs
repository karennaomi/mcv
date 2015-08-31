using System;
using System.Data.Entity;
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
            _listaRepo = new ListaEF(_contexto, new Procedures(_contexto));
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
                if (compraItem.ItemSubstituto != null && compraItem.ItemSubstituto.Motivo != null)
                {
                    _contexto.Entry(compraItem.ItemSubstituto.Motivo).State = EntityState.Unchanged;
                }

                if (compraItem is ListaCompraItem)
                {
                    var listaCompraItem = compraItem as ListaCompraItem;
                    listaCompraItem.Item = listaCompraItem.Item.Id == 0 ? CriarItemNaLista(listaCompraItem, lista) : listaItens.Single(i => i.Id == listaCompraItem.Item.Id);
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
            var produto = ObterProdutoDoCompraItem(compraItem);

            var listaItem = lista.Itens.FirstOrDefault(i => i.Produto.Id == produto.Id);
            
            if(listaItem == null)
            {
                listaItem = new ListaItem
                {
                    Periodo = _contexto.Set<Periodo>().Single(p => p.Nome.Equals("eventual", StringComparison.InvariantCultureIgnoreCase)),
                    QuantidadeConsumo = compraItem.Quantidade,
                    QuantidadeEstoque = compraItem.Quantidade,
                    Produto = produto
                };
                return _listaRepo.AdicionarItem(lista, listaItem, _novaCompra.Integrante.Usuario.Id);
            }
            listaItem.Status = "A";
            listaItem.QuantidadeEstoque = compraItem.Quantidade;
            listaItem.DataAlteracao = DateTime.Now;
            
            return listaItem;
        }

        private static Produto ObterProdutoDoCompraItem(CompraItem compraItem)
        {
            Produto produto;
            var listaCompraItem = compraItem as ListaCompraItem;
            if (listaCompraItem != null)
            {
                produto = listaCompraItem.Item.Produto;
            }
            else
            {
                var item = compraItem as PedidoCompraItem;
                if (item != null)
                {
                    produto = item.Item.Produto;
                }
                else
                {
                    throw new ApplicationException("Tipo de compra item inválido.");
                }
            }
            return produto;
        }
    }
}
