namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioCompraAtiva
    {
        CompraAtiva Obter(long pontoDemandaId, long usuarioId);
        CompraAtiva AtivarCompra(long pontoDemandaId, long usuarioId);
        CompraAtiva FinalizarCompra(CompraAtiva compraAtiva);
    }
}
