
namespace LM.Core.Domain.Repositorio
{
    public interface IUnitOfWork
    {
        IRepositorioIntegrante IntegranteRepo { get; }
        IRepositorioPontoDemanda PontoDemandaRepo { get; }
        void SalvarAlteracoes();
    }
}
