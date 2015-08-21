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
        private readonly IRepositorioProcedures _repoProcedures;
        public CompraEF(IRepositorioProcedures repoProcedures)
        {
            _contexto = new ContextoEF();
            _repoProcedures = repoProcedures;
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
            foreach (var compraItem in compra.Itens)
            {
                _repoProcedures.LancarEstoque(compra.PontoDemanda.Id, 1, compraItem.ProdutoId, compraItem.Quantidade, compra.Integrante.Id);
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
                compraItem.ProdutoId = produto.Id;
                _repoProcedures.InserirProdutoNaFila(produto.Ean, produto.Nome());
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

        public IEnumerable<MotivoSubstituicao> MotivosSubstituicao()
        {
            return _contexto.MotivosSubstituicao;
        }

        public IEnumerable<Compra> Listar(long pontoDemandaId)
        {
            return _contexto.Compras.Where(c => c.PontoDemanda.Id == pontoDemandaId);
        }

        public void RecalcularSugestao(long pontoDemandaId)
        {
            _repoProcedures.RecalcularSugestao(pontoDemandaId);
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}
