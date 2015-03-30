
namespace LM.Core.Domain.Repositorio
{
    public interface IUnitOfWork<out T>
    {
        void SalvarAlteracoes();
        T Contexto { get; }
    }
}
