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
            var usuario = _contexto.Usuarios.SingleOrDefault(u => u.Email == email);
            if (usuario == null) throw new ApplicationException("Usuário não encontrado, email " + email);
            return usuario;
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

        public void VerificarSeCpfJaExiste(string cpf)
        {
            if (_contexto.Usuarios.AsNoTracking().Any(u => u.Cpf == cpf)) throw new UsuarioExistenteException("Cpf");
        }

        public void VerificarSeEmailJaExiste(string email)
        {
            if (_contexto.Usuarios.AsNoTracking().Any(u => u.Email == email)) throw new UsuarioExistenteException("Email");
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
