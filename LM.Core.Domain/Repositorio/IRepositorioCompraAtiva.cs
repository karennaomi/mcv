namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioCompraAtiva
    {
        CompraAtiva Obter(long usuarioId, long pontoDemandaId);
        CompraAtiva AtivarCompra(long usuarioId, long pontoDemandaId);
        CompraAtiva FinalizarCompra(CompraAtiva compraAtiva);
    }
}
