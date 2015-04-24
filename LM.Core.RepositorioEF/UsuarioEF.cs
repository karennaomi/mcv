using System.Data.Entity.Core;
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
            return _contexto.Usuarios.Find(id);
        }

        public Usuario ObterPorEmail(string email)
        {
            return _contexto.Usuarios.SingleOrDefault(u => u.Email == email);
        }

        public Usuario Criar(Usuario usuario)
        {
            if (usuario.Integrante != null) _contexto.Entry(usuario.Integrante.Persona).State = EntityState.Unchanged;
            usuario = _contexto.Usuarios.Add(usuario);
            return usuario;
        }

        public Integrante UsuarioConvidado(string email)
        {
            return _contexto.Integrantes.FirstOrDefault(i => i.EmailConvite == email && i.EhUsuarioConvidado && i.Usuario == null);
        }

        public void AtualizarStatusCadastro(Usuario usuario, StatusCadastro statusCadastro, long? pontoDemandaId = null)
        {
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

        public void AtualizarDeviceInfo(Usuario usuario, string deviceType, string deviceId)
        {
            usuario.DeviceType = deviceType;
            usuario.DeviceId = deviceId;
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}
