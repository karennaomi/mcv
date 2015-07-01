using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class UsuarioEF : IRepositorioUsuario
    {
        private readonly ContextoEF _contexto;
        public UsuarioEF(ContextoEF contexto)
        {
            _contexto = contexto;
        }
        public UsuarioEF()
        {
            _contexto = new ContextoEF();
        }

        public Usuario Obter(long id)
        {
            var usuario = _contexto.Usuarios.Find(id);
            if (usuario == null) throw new ObjetoNaoEncontradoException(LMResource.Usuario_NaoEncontrado);
            return usuario;
        }

        public Usuario ObterPorLogin(string login)
        {
            return _contexto.Usuarios.SingleOrDefault(u => u.Login == login);
        }

        public Usuario Criar(Usuario usuario)
        {
            usuario = _contexto.Usuarios.Add(usuario);
            return usuario;
        }

        public Integrante UsuarioConvidado(string email)
        {
            return _contexto.Integrantes.FirstOrDefault(i => i.Email == email && i.EhUsuarioConvidado && i.Usuario == null);
        }

        public void VerificarSeCpfJaExiste(string cpf)
        {
            if (_contexto.Integrantes.AsNoTracking().Any(i => i.Cpf == cpf)) throw new IntegranteExistenteException("Cpf", "Cpf");
        }

        public void VerificarSeEmailJaExiste(string email)
        {
            if (_contexto.Integrantes.AsNoTracking().Any(i => i.Email == email && !i.EhUsuarioConvidado)) throw new IntegranteExistenteException("Email", "E-mail");
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}
