using LM.Core.Domain;
using System.Data.Entity;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class ComandoCriarCompra
    {
        private readonly ContextoEF _contexto;
        private readonly CompraAtivaEF _compraAtivaRepo;
        private Compra _novaCompra;
        public ComandoCriarCompra(ContextoEF contexto, Compra novaCompra)
        {
            _contexto = contexto;
            _compraAtivaRepo = new CompraAtivaEF(contexto);
            _novaCompra = novaCompra;
        }

        public Compra Executar()
        {
            ChecarItensNovos();
            ChecarItens();

            ChecarIntegrante();
            ChecarPontoDemanda();
            
            _novaCompra = _contexto.Compras.Add(_novaCompra);

            _compraAtivaRepo.FinalizarCompraAtiva(_novaCompra.Integrante.Usuario.Id, _novaCompra.PontoDemanda.Id);

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

        private void ChecarItensNovos()
        {
            var listaRepo = new ListaEF(_contexto);
            var compraItensNovos = _novaCompra.Itens.OfType<ListaCompraItem>().Where(i => i.Item.Id == 0).ToList();
            foreach (var compraItemNovo in compraItensNovos)
            {
                compraItemNovo.Item.Periodo = new Periodo {Id = 12 /* Eventual */};
                compraItemNovo.Item.QuantidadeDeConsumo = compraItemNovo.Quantidade;
                compraItemNovo.Item.QuantidadeEmEstoque = compraItemNovo.Quantidade;
                var lista = listaRepo.ObterListaPorPontoDemanda(_novaCompra.PontoDemanda.Id);
                compraItemNovo.Item = listaRepo.AdicionarItem(lista, compraItemNovo.Item);
            }
        }

        private void ChecarItens()
        {
            var listaItens = _contexto.Listas.Single(l => l.PontoDemanda.Id == _novaCompra.PontoDemanda.Id).Itens;
            foreach (var compraItem in _novaCompra.Itens)
            {
                if (compraItem is ListaCompraItem)
                {
                    var listaCompraItem = compraItem as ListaCompraItem;
                    listaCompraItem.Item = listaItens.Single(i => i.Id == listaCompraItem.Item.Id);
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
