using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class CompraEF : IRepositorioCompra
    {
        private readonly IUnitOfWork<ContextoEF> _uniOfWork;
        public CompraEF(IUnitOfWork<ContextoEF> uniOfWork)
        {
            _uniOfWork = uniOfWork;
        }

        public Compra Criar(Compra compra)
        {
            _uniOfWork.Contexto.Entry(compra.Integrante).State = EntityState.Unchanged;
            _uniOfWork.Contexto.Entry(compra.PontoDemanda).State = EntityState.Unchanged;

            var lista = _uniOfWork.Contexto.Listas.Local.FirstOrDefault(l => l.PontoDemanda.Id == compra.PontoDemanda.Id);
            foreach (var compraItem in compra.Itens)
            {
                if (compraItem is ListaCompraItem)
                {
                    var listaCompraItem = compraItem as ListaCompraItem;
                    if (lista != null)
                    {
                        var itemLocal = lista.Itens.FirstOrDefault(i => i.Id == listaCompraItem.Item.Id);
                        if (itemLocal == null) continue;
                        listaCompraItem.ProdutoId = itemLocal.Produto.Id;
                        listaCompraItem.Item = itemLocal;
                    }
                    else if (listaCompraItem.Item.Id > 0)
                    {
                        _uniOfWork.Contexto.Entry(listaCompraItem.Item).State = EntityState.Unchanged;
                    }
                    
                }
                else if(compraItem is PedidoCompraItem)
                {
                    var pedidoCompraItem = compraItem as PedidoCompraItem;
                    _uniOfWork.Contexto.Entry(pedidoCompraItem.Item).State = EntityState.Unchanged;
                } 
            }
            
            compra = _uniOfWork.Contexto.Compras.Add(compra);
            _uniOfWork.Contexto.SaveChanges();

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
                _uniOfWork.Contexto.Database.ExecuteSqlCommand("INSERT INTO [dbo].[TB_Compra_Item_Pedido] VALUES(@compraItemId, @pedidoItem)", new SqlParameter("@compraItemId", compraItem.Id), new SqlParameter("@pedidoItem", compraItem.Item.Id));
            }
        }
    }
}
