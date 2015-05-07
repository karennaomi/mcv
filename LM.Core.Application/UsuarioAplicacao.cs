using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;

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
            return _repositorio.ObterPorLogin(login);
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
                usuario.StatusUsuarioPontoDemanda.Add(new StatusUsuarioPontoDemanda { StatusCadastro = StatusCadastro.UsuarioConvidado });
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
            var usuario = _repositorio.ObterPorLogin(login);
            if (PasswordHash.ValidatePassword(senha, usuario.Senha))
            {
                return usuario;
            }
            throw new LoginInvalidoException();
        }

        public void AtualizarStatusCadastro(long usuarioId, StatusCadastro statusCadastro, long? pontoDemandaId = null)
        {
            var usuario = Obter(usuarioId);
            _repositorio.AtualizarStatusCadastro(usuario, statusCadastro, pontoDemandaId);
            _repositorio.Salvar();
        }

        public void AtualizarDeviceInfo(long usuarioId, string deviceType, string deviceId)
        {
            var usuario = Obter(usuarioId);
            _repositorio.AtualizarDeviceInfo(usuario, deviceType, deviceId);
            _repositorio.Salvar();
        }
    }
}



