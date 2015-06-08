using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;
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

        public Compra Obter(long pontoDemandaId, long id)
        {
            return _contexto.Compras.SingleOrDefault(c => c.Id == id && c.PontoDemanda.Id == pontoDemandaId);
        }

        public Compra Criar(Compra compra)
        {
            return new ComandoCriarCompra(_contexto, compra).Executar();
        }

        public void LancarEstoque(Compra compra)
        {
            var lancamentoEstoque = new LancamentoEstoque(_contexto);
            foreach (var compraItem in compra.Itens)
            {
                lancamentoEstoque.Lancar(compra.PontoDemanda.Id, 1, compraItem.ProdutoId, compraItem.Quantidade, compra.Integrante.Id);
            }
        }

        public void PreencherProdutoId(IEnumerable<CompraItem> itens)
        {
            foreach (var compraItem in itens.Where(i => i.ProdutoId  == null || i.ProdutoId == 0))
            {
                var produto = compraItem is ListaCompraItem ? ((ListaCompraItem) compraItem).Item.Produto : ((PedidoCompraItem) compraItem).Item.Produto;
                var produtoIdParam = new SqlParameter("@produtoId", produto.Id);
                var compraItemIdParam = new SqlParameter("@id", compraItem.Id);
                _contexto.Database.ExecuteSqlCommand("UPDATE [dbo].[TB_Compra_Item] SET ID_PRODUTO = @produtoId WHERE ID_COMPRA_ITEM = @id", produtoIdParam, compraItemIdParam);

                var eanParam = new SqlParameter("@EAN", produto.Ean);
                var produtoNomeParam = new SqlParameter("@NomeProduto", produto.Nome());
                _contexto.Database.ExecuteSqlCommand("SP_INSERE_PRODUTO_NOVO_FILA @EAN, @NomeProduto", eanParam, produtoNomeParam);

                compraItem.ProdutoId = produto.Id;
            }
        }

        public void PreencheTabelaRelacionamentoCompraPedido(IEnumerable<PedidoCompraItem> itens)
        {
            foreach (var compraItem in itens)
            {
                var compraItemIdParam = new SqlParameter("@compraItemId", compraItem.Id);
                var itemIdParam = new SqlParameter("@pedidoItem", compraItem.Item.Id);
                _contexto.Database.ExecuteSqlCommand("INSERT INTO [dbo].[TB_Compra_Item_Pedido] VALUES(@compraItemId, @pedidoItem)", compraItemIdParam, itemIdParam);
            }
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}
