using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.Application
{
    public interface IUsuarioAplicacao
    {
        Usuario Obter(int id);
        Usuario Obter(string email);
        Usuario Criar(Usuario usuario);
        Usuario ValidarLogin(string email, string senha);
        void AtualizaStatusCadastro(long usuarioId, StatusCadastro statusCadastro, long? pontoDemandaId = null);
    }

    public class UsuarioAplicacao : IUsuarioAplicacao
    {
        private readonly IRepositorioUsuario _repositorio;
        public UsuarioAplicacao(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }

        public Usuario Obter(int id)
        {
            return _repositorio.Obter(id);
        }

        public Usuario Obter(string email)
        {
            return _repositorio.ObterPorEmail(email);
        }

        public Usuario Criar(Usuario usuario)
        {
            _repositorio.VerificarSeCpfJaExiste(usuario.Cpf);
            usuario.Login = usuario.Email;
            if(usuario.StatusUsuarioPontoDemanda == null) usuario.StatusUsuarioPontoDemanda = new StatusUsuarioPontoDemanda();
            usuario.StatusUsuarioPontoDemanda.StatusCadastro = StatusCadastro.EtapaDeInformacoesPessoaisCompleta;
            return _repositorio.Criar(usuario);
        }

        public Usuario ValidarLogin(string email, string senha)
        {
            return _repositorio.ValidarLogin(email, senha);
        }

        public void AtualizaStatusCadastro(long usuarioId, StatusCadastro statusCadastro, long? pontoDemandaId = null)
        {
            _repositorio.AtualizarStatusCadastro(usuarioId, statusCadastro, pontoDemandaId);
        }
    }
}



