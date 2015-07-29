
namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioProcedures
    {
        void LancarEstoque(long pontoDemandaId, int origem, int? produtoId, decimal? quantidade, long integranteId);
        void InserirProdutoNaFila(string ean, string nome);
    }
}
