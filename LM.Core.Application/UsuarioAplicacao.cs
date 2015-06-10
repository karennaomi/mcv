using System;
using System.Collections.ObjectModel;
using System.Linq;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface IUsuarioAplicacao
    {
        Usuario Obter(long id);
        Usuario Obter(string email);
        Usuario Criar(Usuario usuario);
        Usuario Atualizar(Usuario usuario);
        Usuario ValidarLogin(string email, string senha);
        void AtualizarStatusCadastro(long usuarioId, StatusCadastro statusCadastro, long? pontoDemandaId = null);
        void AtualizarDeviceInfo(long usuarioId, string deviceType, string deviceId);
    }

    public class UsuarioAplicacao : IUsuarioAplicacao
    {
        private readonly IRepositorioUsuario _repositorio;
        public UsuarioAplicacao(IRepositorioUsuario repositorio)
        {
            _repositorio = repositorio;
        }

        public Usuario Obter(long id)
        {
            return _repositorio.Obter(id);
        }

        public Usuario Obter(string login)
        {
            var usuario = _repositorio.ObterPorLogin(login);
            if (usuario == null) throw new ObjetoNaoEncontradoException("Usuário não encontrado, login " + login);
            return usuario;
        }

        public Usuario Criar(Usuario usuario)
        {
            if (!string.IsNullOrWhiteSpace(usuario.Integrante.Cpf)) _repositorio.VerificarSeCpfJaExiste(usuario.Integrante.Cpf);
            _repositorio.VerificarSeEmailJaExiste(usuario.Integrante.Email);
            usuario.Login = usuario.Integrante.Email;
            usuario.Senha = PasswordHash.CreateHash(usuario.Senha); 
            if(usuario.StatusUsuarioPontoDemanda == null) usuario.StatusUsuarioPontoDemanda = new List<StatusUsuarioPontoDemanda>();

            var integrante = _repositorio.UsuarioConvidado(usuario.Integrante.Email);
            if (integrante != null)
            {
                integrante.EhUsuarioConvidado = false;
                usuario.Integrante = integrante;
                usuario.StatusUsuarioPontoDemanda.Add(new StatusUsuarioPontoDemanda { StatusCadastro = StatusCadastro.UsuarioConvidado });
                usuario.StatusUsuarioPontoDemanda.Add(new StatusUsuarioPontoDemanda { StatusCadastro = StatusCadastro.UsuarioOk });
            }
            else
            {
                usuario.StatusUsuarioPontoDemanda.Add(new StatusUsuarioPontoDemanda { StatusCadastro = StatusCadastro.EtapaDeInformacoesPessoaisCompleta });
            }

            _repositorio.Criar(usuario);
            _repositorio.Salvar();
            return usuario;
        }

        public Usuario Atualizar(Usuario usuario)
        {
            var usuarioToUpdate = Obter(usuario.Id);
            if (usuarioToUpdate.Integrante.Email != usuario.Integrante.Email) _repositorio.VerificarSeEmailJaExiste(usuario.Integrante.Email);
            usuarioToUpdate.Integrante.Atualizar(usuario.Integrante);
            _repositorio.Salvar();
            return usuarioToUpdate;
        }

        public Usuario ValidarLogin(string login, string senha)
        {
            var usuario = Obter(login);
            if (!usuario.Ativo) throw new LoginInvalidoException("Usuário desativado.");
            if (PasswordHash.ValidatePassword(senha, usuario.Senha)) return usuario;
            throw new LoginInvalidoException();
        }

        public void AtualizarStatusCadastro(long usuarioId, StatusCadastro statusCadastro, long? pontoDemandaId = null)
        {
            var usuario = Obter(usuarioId);
            if (usuario.StatusUsuarioPontoDemanda != null && usuario.StatusUsuarioPontoDemanda.Any(s => s.StatusCadastro == StatusCadastro.UsuarioOk)) return;
            if (usuario.StatusUsuarioPontoDemanda == null) usuario.StatusUsuarioPontoDemanda=new Collection<StatusUsuarioPontoDemanda>();
            usuario.StatusUsuarioPontoDemanda.Add(new StatusUsuarioPontoDemanda
            {
                StatusCadastro = statusCadastro,
                DataInclusao = DateTime.Now,
                DataAlteracao = DateTime.Now,
                PontoDemandaId = pontoDemandaId
            });
            _repositorio.Salvar();
        }

        public void AtualizarDeviceInfo(long usuarioId, string deviceType, string deviceId)
        {
            var usuario = Obter(usuarioId);
            //TODO: anular todos os devices ids que existir igual ao enviado
            usuario.DeviceType = deviceType;
            usuario.DeviceId = deviceId;
            _repositorio.Salvar();
        }
    }
}



