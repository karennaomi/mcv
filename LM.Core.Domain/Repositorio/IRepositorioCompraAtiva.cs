namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioCompraAtiva
    {
        CompraAtiva Obter(long pontoDemandaId);
        CompraAtiva AtivarCompra(long usuarioId, long pontoDemandaId);
        CompraAtiva FinalizarCompra(long usuarioId, long pontoDemandaId);
    }
}
