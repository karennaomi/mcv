using System.Runtime.InteropServices;
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
        private readonly ContextoEF _contexto;
        public CompraEF()
        {
            _contexto = new ContextoEF();
        }

        public Compra Criar(Compra compra)
        {
            compra = new ComandoCriarCompra(_contexto, compra).Executar();
            Salvar();
            PreencherProdutoId(compra.Itens); //Campo desnecessário no modelo, mas quem fez não sabe dizer onde é usado ou mudar para usar o produto associado ao item se necessário
            PreencheTabelaRelacionamentoCompraPedido(compra.Itens.OfType<PedidoCompraItem>()); //Aqui o id do pedido foi movido para a propria tabela de item da compra  mas como não sabem onde mudar nas procs preencho essa tabela pra não quebrar procs
            /*
                INSERT INTO dbo.TB_FILA ( ID_TIPO_OPERACAO, ID_TIPO_SERVICO,  FL_STATUS_PROCESSAMENTO , TX_FILA_ORIGEM , DT_INC) VALUES ( 2 , 2 , 'A' , 'CADASTRO DO PRODUTO' , GETDATE())
	            SELECT @ID_FILA = SCOPE_IDENTITY()
            	INSERT INTO dbo.TB_FILA_PRODUTO(ID_FILA, CD_PRODUTO_EAN, TX_DESCRICAO_PRODUTO) VALUES (@ID_FILA, @EAN, @NomeProduto)
            */
            return compra;
        }

        private void PreencherProdutoId(IEnumerable<CompraItem> itens)
        {
            foreach (var compraItem in itens.Where(i => i.ProdutoId  == null || i.ProdutoId == 0))
            {
                var produtoId = compraItem is ListaCompraItem ? ((ListaCompraItem) compraItem).Item.Produto.Id : ((PedidoCompraItem) compraItem).Item.Produto.Id;
                _contexto.Database.ExecuteSqlCommand("UPDATE [dbo].[TB_Compra_Item] SET ID_PRODUTO = @produtoId WHERE ID_COMPRA_ITEM = @id", new SqlParameter("@produtoId", produtoId), new SqlParameter("@id", compraItem.Id));
            }
        }

        private void PreencheTabelaRelacionamentoCompraPedido(IEnumerable<PedidoCompraItem> itens)
        {
            foreach (var compraItem in itens)
            {
                _contexto.Database.ExecuteSqlCommand("INSERT INTO [dbo].[TB_Compra_Item_Pedido] VALUES(@compraItemId, @pedidoItem)", new SqlParameter("@compraItemId", compraItem.Id), new SqlParameter("@pedidoItem", compraItem.Item.Id));
            }
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}
