using System;
using System.Data.SqlClient;

namespace LM.Core.RepositorioEF
{
    public class LancamentoEstoque
    {
        private readonly ContextoEF _contexto;
        public LancamentoEstoque(ContextoEF contexto)
        {
            _contexto = contexto;
        }

        public void Lancar(long pontoDemandaId, int origem, int? produtoId, decimal? quantidade, long integranteId)
        {
            var pontoDemandaIdParam = new SqlParameter("@IDPReD", pontoDemandaId);
            var origemParam = new SqlParameter("@IDOrigemLancamentoEstoque", origem);
            var produtoIdParam = new SqlParameter("@IDProduto", produtoId);
            var quantidadeParam = new SqlParameter("@QtLancada", quantidade);
            var integranteIdParam = new SqlParameter("@IDIntegrante", integranteId);
            _contexto.Database.ExecuteSqlCommand("SP_APP_EFETUA_LANCAMENTO_ESTOQUE @IDPReD, @IDOrigemLancamentoEstoque, @IDProduto, @QtLancada, @IDIntegrante", pontoDemandaIdParam, origemParam,
                produtoIdParam, quantidadeParam, integranteIdParam);
        }
    }
}
