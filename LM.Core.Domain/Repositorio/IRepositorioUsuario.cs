
namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioUsuario
    {
        Usuario Obter(long id);
        Usuario ObterPorEmail(string email);
        Usuario Criar(Usuario usuario);
        Integrante UsuarioConvidado(string email);
        void AtualizarStatusCadastro(Usuario usuario, StatusCadastro statusCadastro, long? pontoDemandaId = null);
        void VerificarSeCpfJaExiste(string cpf);
        void VerificarSeEmailJaExiste(string email);
        void AtualizarDeviceInfo(Usuario usuario, string deviceType, string deviceId);
        void Salvar();
    }
}
