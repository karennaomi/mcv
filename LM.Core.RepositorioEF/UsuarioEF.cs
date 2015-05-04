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
            var usuario = _contexto.Usuarios.Find(id);
            if (usuario == null) throw new ObjetoNaoEncontradoException("Usuário não encontrado, id " + id);
            return usuario;
        }

        public Usuario ObterPorEmail(string email)
        {
            var usuario = _contexto.Usuarios.SingleOrDefault(u => u.Email == email);
            if (usuario == null) throw new ObjetoNaoEncontradoException("Usuário não encontrado, email " + email);
            return usuario;
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
            if (usuario.StatusUsuarioPontoDemanda.Any(s => s.StatusCadastro == StatusCadastro.UsuarioOk)) return;
            usuario.StatusUsuarioPontoDemanda.Add(new StatusUsuarioPontoDemanda
            {
                StatusCadastro = statusCadastro,
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now,
                PontoDemandaId = pontoDemandaId
            });
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
