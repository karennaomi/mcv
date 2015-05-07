
namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioUsuario
    {
        Usuario Obter(long id);
        Usuario ObterPorLogin(string login);
        Usuario Criar(Usuario usuario);
        Integrante UsuarioConvidado(string email);
        void AtualizarStatusCadastro(Usuario usuario, StatusCadastro statusCadastro, long? pontoDemandaId = null);
        void AtualizarDeviceInfo(Usuario usuario, string deviceType, string deviceId);
        void VerificarSeCpfJaExiste(string cpf);
        void VerificarSeEmailJaExiste(string email);
        void Salvar();
    }
}
