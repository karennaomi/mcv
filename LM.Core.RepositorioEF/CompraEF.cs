using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
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
                else if (compraItem is ProdutoCompraItem)
                {
                    var produtoCompraItem = compraItem as ProdutoCompraItem;
                    foreach (var categoria in produtoCompraItem.Produto.Categorias)
                    {
                        _contexto.Entry(categoria).State = EntityState.Unchanged;
                    }
                }
            }
            _contexto.SaveChanges();

            PreencheTabelaRelacionamentoCompraPedido(compra.Itens.OfType<PedidoCompraItem>());
            /*
                INSERT INTO dbo.TB_FILA ( ID_TIPO_OPERACAO, ID_TIPO_SERVICO,  FL_STATUS_PROCESSAMENTO , TX_FILA_ORIGEM , DT_INC) VALUES ( 2 , 2 , 'A' , 'CADASTRO DO PRODUTO' , GETDATE())

	            SELECT @ID_FILA = SCOPE_IDENTITY()
	
            	INSERT INTO dbo.TB_FILA_PRODUTO(ID_FILA, CD_PRODUTO_EAN, TX_DESCRICAO_PRODUTO) VALUES (@ID_FILA, @EAN, @NomeProduto)
             */

            return compra;
        }

        private void PreencheTabelaRelacionamentoCompraPedido(IEnumerable<PedidoCompraItem> itens)
        {
            foreach (var compraItem in itens)
            {
                _contexto.Database.ExecuteSqlCommand("INSERT INTO [dbo].[TB_Compra_Item_Pedido] VALUES(@compraItemId, @pedidoItem)", new SqlParameter("@compraItemId", compraItem.Id), new SqlParameter("@pedidoItem", compraItem.Item.Id));
            }
        }
    }
}
