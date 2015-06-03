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
            compra = new ComandoCriarCompra(_contexto, compra).Executar();
            Salvar();
            PreencherProdutoId(compra.Itens); //Campo desnecessário no modelo, mas quem fez não sabe dizer onde é usado ou mudar para usar o produto associado ao item se necessário
            PreencheTabelaRelacionamentoCompraPedido(compra.Itens.OfType<PedidoCompraItem>()); //Aqui o id do pedido foi movido para a propria tabela de item da compra  mas como não sabem onde mudar nas procs preencho essa tabela pra não quebrar procs
            LancarEstoque(compra);
            return compra;
        }

        private void LancarEstoque(Compra compra)
        {
            foreach (var compraItem in compra.Itens)
            {
                var pontoDemandaIdParam = new SqlParameter("@IDPReD", compra.PontoDemanda.Id);
                var origemParam = new SqlParameter("@IDOrigemLancamentoEstoque", 1);
                var produtoIdParam = new SqlParameter("@IDProduto", compraItem.ProdutoId);
                var quantidadeParam = new SqlParameter("@QtLancada", compraItem.Quantidade);
                var integranteIdParam = new SqlParameter("@IDIntegrante", compra.Integrante.Id);
                _contexto.Database.ExecuteSqlCommand("SP_APP_EFETUA_LANCAMENTO_ESTOQUE @IDPReD, @IDOrigemLancamentoEstoque, @IDProduto, @QtLancada, @IDIntegrante", pontoDemandaIdParam, origemParam,
                    produtoIdParam, quantidadeParam, integranteIdParam);
            }
        }

        private void PreencherProdutoId(IEnumerable<CompraItem> itens)
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

        private void PreencheTabelaRelacionamentoCompraPedido(IEnumerable<PedidoCompraItem> itens)
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
