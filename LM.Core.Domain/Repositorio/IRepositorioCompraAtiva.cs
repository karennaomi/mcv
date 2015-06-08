namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioCompraAtiva
    {
        CompraAtiva Obter(long pontoDemandaId);
        CompraAtiva AtivarCompra(long usuarioId, long pontoDemandaId);
        void FinalizarCompraAtiva(long usuarioId, long pontoDemandaId);
        void Salvar();
    }
}
