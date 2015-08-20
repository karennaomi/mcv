using System.Data.SqlClient;
using LM.Core.Domain.Repositorio;

namespace LM.Core.RepositorioEF
{
    public class Procedures : IRepositorioProcedures
    {
        private readonly ContextoEF _contexto;
        public Procedures()
        {
            _contexto = new ContextoEF();
        }
        public Procedures(ContextoEF contexto)
        {
            _contexto = contexto;
        }

        public void LancarEstoque(long pontoDemandaId, int origem, int? produtoId, decimal? quantidade, long integranteId)
        {
            var pontoDemandaIdParam = new SqlParameter("@IDPReD", pontoDemandaId);
            var origemParam = new SqlParameter("@IDOrigemLancamentoEstoque", origem);
            var produtoIdParam = new SqlParameter("@IDProduto", produtoId);
            var quantidadeParam = new SqlParameter("@QtLancada", quantidade);
            var integranteIdParam = new SqlParameter("@IDIntegrante", integranteId);
            _contexto.Database.ExecuteSqlCommandAsync("SP_APP_EFETUA_LANCAMENTO_ESTOQUE @IDPReD, @IDOrigemLancamentoEstoque, @IDProduto, @QtLancada, @IDIntegrante", pontoDemandaIdParam, origemParam,
                produtoIdParam, quantidadeParam, integranteIdParam);
        }


        public void InserirProdutoNaFila(string ean, string nome)
        {
            var eanParam = new SqlParameter("@EAN", ean);
            var produtoNomeParam = new SqlParameter("@NomeProduto", nome);
            _contexto.Database.ExecuteSqlCommandAsync("SP_INSERE_PRODUTO_NOVO_FILA @EAN, @NomeProduto", eanParam, produtoNomeParam);
        }


        public void RecalcularSugestao(long pontoDemandaId, int? produtoId)
        {
            var pontoDemandaIdParam = new SqlParameter("@ID_PONTO_REAL_DEMANDA", pontoDemandaId);
            var produtoIdParam = new SqlParameter("@ID_PRODUTO", produtoId);
            _contexto.Database.ExecuteSqlCommandAsync(
                "SP_SPSS_CALCULO_SUGESTAO_PONTO_DEMANDA @ID_PONTO_REAL_DEMANDA, @ID_PRODUTO", pontoDemandaIdParam,
                produtoIdParam);
        }
    }
}
