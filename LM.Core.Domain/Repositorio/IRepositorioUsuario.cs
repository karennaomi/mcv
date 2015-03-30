
namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioUsuario
    {
        Usuario Obter(long id);
        Usuario ObterPorEmail(string email);
        Usuario Criar(Usuario usuario);
        void AtualizarStatusCadastro(long usuarioId, StatusCadastro statusCadastro, long? pontoDemandaId = null);
        bool VerificarSeCpfJaExiste(string cpf);
        Usuario ValidarLogin(string email, string senha);
        void AtualizarDeviceInfo(long usuarioId, string deviceType, string deviceId);
    }
}
