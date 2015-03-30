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
        private readonly IUnitOfWork<ContextoEF> _uniOfWork;
        public UsuarioEF(IUnitOfWork<ContextoEF> uniOfWork)
        {
            _uniOfWork = uniOfWork;
        }

        public Usuario Obter(long id)
        {
            return _uniOfWork.Contexto.Usuarios.Find(id);
        }

        public Usuario ObterPorEmail(string email)
        {
            return _uniOfWork.Contexto.Usuarios.SingleOrDefault(u => u.Email == email);
        }

        public Usuario Criar(Usuario usuario)
        {
            _uniOfWork.Contexto.Entry(usuario.Integrante.Persona).State = EntityState.Unchanged;
            usuario = _uniOfWork.Contexto.Usuarios.Add(usuario);
            _uniOfWork.Contexto.SaveChanges();
            return usuario;
        }

        public void AtualizarStatusCadastro(long usuarioId, StatusCadastro statusCadastro, long? pontoDemandaId = null)
        {
            var usuario = Obter(usuarioId);
            usuario.StatusUsuarioPontoDemanda.StatusCadastro = statusCadastro;
            usuario.StatusUsuarioPontoDemanda.DataAlteracao = DateTime.Now;
            if (pontoDemandaId != null && pontoDemandaId > 0)
            {
                usuario.StatusUsuarioPontoDemanda.PontoDemandaId = pontoDemandaId;
            }
            _uniOfWork.Contexto.SaveChanges();
        }        

        public bool VerificarSeCpfJaExiste(string cpf)
        {
            return _uniOfWork.Contexto.Usuarios.Any(u => u.Cpf == cpf);
        }

        public Usuario ValidarLogin(string email, string senha)
        {
            var usuario = _uniOfWork.Contexto.Usuarios.FirstOrDefault(u => u.Login == email && u.Senha == senha);
            if(usuario == null) throw new LoginInvalidoException();
            return usuario;
        }

        public void AtualizarDeviceInfo(long usuarioId, string deviceType, string deviceId)
        {
            var usuario = Obter(usuarioId);
            usuario.DeviceType = deviceType;
            usuario.DeviceId = deviceId;
            _uniOfWork.Contexto.SaveChanges();
        }
    }
}
