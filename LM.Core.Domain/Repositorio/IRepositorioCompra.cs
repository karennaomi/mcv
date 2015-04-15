
namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioCompra
    {
        Compra Obter(long pontoDemandaId, long id);
        Compra Criar(Compra compra);
        void Salvar();
    }
}
