
namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioCompra
    {
        Compra Criar(Compra compra);
        void Salvar();
    }
}
