
namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioIntegrante
    {
        Integrante Criar(Integrante integrante);
        void Apagar(long id);
    }
}
