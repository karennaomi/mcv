using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using System;
using System.Data.Entity;
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
            var usuario =_contexto.Usuarios.Find(id);
            if (usuario == null) throw new ApplicationException("Usuário não encontrado, id " + id);
            return usuario;
        }

        public Usuario ObterPorEmail(string email)
        {
            return _contexto.Usuarios.SingleOrDefault(u => u.Email == email);
        }

        public Usuario Criar(Usuario usuario)
        {
            _contexto.Entry(usuario.Integrante.Persona).State = EntityState.Unchanged;
            usuario = _contexto.Usuarios.Add(usuario);
            return usuario;
        }

        public void AtualizarStatusCadastro(long usuarioId, StatusCadastro statusCadastro, long? pontoDemandaId = null)
        {
            var usuario = Obter(usuarioId);
            var statusUsuarioPontoDemanda = (pontoDemandaId.HasValue ? usuario.StatusUsuarioPontoDemanda.SingleOrDefault(s => s.PontoDemandaId == pontoDemandaId) : usuario.StatusUsuarioPontoDemanda.First()) ?? usuario.StatusUsuarioPontoDemanda.First();
            statusUsuarioPontoDemanda.StatusCadastro = statusCadastro;
            statusUsuarioPontoDemanda.DataAlteracao = DateTime.Now;
            statusUsuarioPontoDemanda.PontoDemandaId = pontoDemandaId;
        }        

        public bool VerificarSeCpfJaExiste(string cpf)
        {
            return _contexto.Usuarios.AsNoTracking().Any(u => u.Cpf == cpf);
        }

        public Usuario ValidarLogin(string email, string senha)
        {
            var usuario = _contexto.Usuarios.AsNoTracking().FirstOrDefault(u => u.Login == email && u.Senha == senha);
            if(usuario == null) throw new LoginInvalidoException();
            return usuario;
        }

        public void AtualizarDeviceInfo(long usuarioId, string deviceType, string deviceId)
        {
            var usuario = Obter(usuarioId);
            usuario.DeviceType = deviceType;
            usuario.DeviceId = deviceId;
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}
