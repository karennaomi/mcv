
namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioUsuario
    {
        Usuario Obter(long id);
        Usuario ObterPorLogin(string login);
        Usuario Criar(Usuario usuario);
        Integrante UsuarioConvidado(string email);
        void VerificarSeCpfJaExiste(string cpf);
        void VerificarSeEmailJaExiste(string email);
        void Salvar();
    }
}
