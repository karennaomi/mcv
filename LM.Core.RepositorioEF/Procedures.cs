using System;
using LM.Core.Domain.Repositorio;
using System.Data.SqlClient;

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

        public void RecalcularSugestao(long pontoDemandaId, int? produtoId = null)
        {
            var pontoDemandaIdParam = new SqlParameter("@ID_PONTO_REAL_DEMANDA", pontoDemandaId);
            var produtoIdParam = produtoId.HasValue ? new SqlParameter("@ID_PRODUTO", produtoId.Value) : new SqlParameter("@ID_PRODUTO", DBNull.Value);
            _contexto.Database.ExecuteSqlCommand("SP_SPSS_CALCULO_SUGESTAO_PONTO_DEMANDA @ID_PONTO_REAL_DEMANDA, @ID_PRODUTO", pontoDemandaIdParam, produtoIdParam);
        }

        public void LancarEstoque(long pontoDemandaId, int origem, int? produtoId, decimal? quantidade, long integranteId)
        {
            var pontoDemandaIdParam = new SqlParameter("@IDPReD", pontoDemandaId);
            var origemParam = new SqlParameter("@IDOrigemLancamentoEstoque", origem);
            var produtoIdParam = produtoId.HasValue ? new SqlParameter("@IDProduto", produtoId) : new SqlParameter("@IDProduto", DBNull.Value);
            var quantidadeParam = quantidade.HasValue ? new SqlParameter("@QtLancada", quantidade) : new SqlParameter("@QtLancada", DBNull.Value);
            var integranteIdParam = new SqlParameter("@IDIntegrante", integranteId);
            _contexto.Database.ExecuteSqlCommand("SP_APP_EFETUA_LANCAMENTO_ESTOQUE @IDPReD, @IDOrigemLancamentoEstoque, @IDProduto, @QtLancada, @IDIntegrante", pontoDemandaIdParam, origemParam,
                produtoIdParam, quantidadeParam, integranteIdParam);
        }

        public void InserirProdutoNaFila(string ean, string nome)
        {
            var eanParam = string.IsNullOrEmpty(ean) ? new SqlParameter("@EAN", DBNull.Value) : new SqlParameter("@EAN", ean);
            var produtoNomeParam = string.IsNullOrEmpty(nome) ? new SqlParameter("@NomeProduto", DBNull.Value) : new SqlParameter("@NomeProduto", nome);
            _contexto.Database.ExecuteSqlCommand("SP_INSERE_PRODUTO_NOVO_FILA @EAN, @NomeProduto", eanParam, produtoNomeParam);
        }
    }
}
