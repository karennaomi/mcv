using System.Data.Entity;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using System;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class UsuarioEF : IRepositorioUsuario
    {
        private readonly ContextoEF _contexto;

        public UsuarioEF()
        {
            _contexto = new ContextoEF();
        }

        public Usuario Obter(long id)
        {
            return _contexto.Usuarios.Find(id);
        }

        public Usuario ObterPorEmail(string email)
        {
            return _contexto.Usuarios.SingleOrDefault(u => u.Email == email);
        }

        public Usuario Criar(Usuario usuario)
        {
            _contexto.Entry(usuario.Integrante.Persona).State = EntityState.Unchanged;
            usuario = _contexto.Usuarios.Add(usuario);
            _contexto.SaveChanges();
            return usuario;
        }

        public void AtualizarStatusCadastro(long usuarioId, StatusCadastro statusCadastro, long? pontoDemandaId = null)
        {
            var usuario = Obter(usuarioId);
            usuario.StatusUsuarioPontoDemanda.StatusCadastro = statusCadastro;
            usuario.StatusUsuarioPontoDemanda.DataAlteracao = DateTime.Now;
            if (pontoDemandaId != null && pontoDemandaId > 0) usuario.StatusUsuarioPontoDemanda.PontoDemandaId = pontoDemandaId;
            _contexto.SaveChanges();
        }        

        public bool VerificarSeCpfJaExiste(string cpf)
        {
            return _contexto.Usuarios.Any(u => u.Cpf == cpf);
        }

        public Usuario ValidarLogin(string email, string senha)
        {
            var usuario = _contexto.Usuarios.FirstOrDefault(u => u.Login == email && u.Senha == senha);
            if(usuario == null) throw new LoginInvalidoException();
            return usuario;
        }

        public void AtualizarDeviceId(long usuarioId, string deviceId)
        {
            var usuario = Obter(usuarioId);
            usuario.DeviceId = deviceId;
            _contexto.SaveChanges();
        }
    }
}
