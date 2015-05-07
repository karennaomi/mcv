
namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioIntegrante
    {
        Integrante Obter(long id);
        Integrante Criar(Integrante integrante);
        void Apagar(long id);
        void VerificarSeCpfJaExiste(string cpf);
        void VerificarSeEmailJaExiste(string email);
        void Salvar();
    }
}
