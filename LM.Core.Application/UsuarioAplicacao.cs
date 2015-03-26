using System.Collections.ObjectModel;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.Application
{
    public interface IUsuarioAplicacao
    {
        Usuario Obter(long id);
        Usuario Obter(string email);
        Usuario Criar(Usuario usuario);
        Usuario ValidarLogin(string email, string senha);
        void AtualizaStatusCadastro(long usuarioId, StatusCadastro statusCadastro, long? pontoDemandaId = null);
    }

    public class UsuarioAplicacao : IUsuarioAplicacao
    {
        private readonly IRepositorioUsuario _repositorio;
        private readonly IIntegranteAplicacao _appIntegrante;
        public UsuarioAplicacao(IRepositorioUsuario repositorio, IIntegranteAplicacao appIntegrante)
        {
            _repositorio = repositorio;
            _appIntegrante = appIntegrante;
        }

        public Usuario Obter(long id)
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
            if (usuario.MapIntegrantes == null) usuario.MapIntegrantes = new Collection<Integrante>();
            usuario.MapIntegrantes.Add(new Integrante(usuario));
            _appIntegrante.Criar(usuario.Integrante);
            return usuario;
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



