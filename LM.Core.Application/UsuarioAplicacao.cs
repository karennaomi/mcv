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
        Usuario ValidarLogin(string email, string senha);
        void AtualizarStatusCadastro(long usuarioId, StatusCadastro statusCadastro, long? pontoDemandaId = null);
        void AtualizarDeviceInfo(long usuarioId, string deviceType, string deviceId);
    }

    public class UsuarioAplicacao : IUsuarioAplicacao
    {
        private readonly IRepositorioUsuario _repositorio;
        private readonly IPersonaAplicacao _appPersona;
        public UsuarioAplicacao(IRepositorioUsuario repositorio, IPersonaAplicacao appPersona)
        {
            _repositorio = repositorio;
            _appPersona = appPersona;
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
            if (!string.IsNullOrWhiteSpace(usuario.Cpf)) _repositorio.VerificarSeCpfJaExiste(usuario.Cpf);
            _repositorio.VerificarSeEmailJaExiste(usuario.Email);
            usuario.Login = usuario.Email;
            usuario.Senha = PasswordHash.CreateHash(usuario.Senha); 
            if(usuario.StatusUsuarioPontoDemanda == null) usuario.StatusUsuarioPontoDemanda = new List<StatusUsuarioPontoDemanda>();

            var integrante = _repositorio.UsuarioConvidado(usuario.Email);
            if (integrante != null)
            {
                usuario.StatusUsuarioPontoDemanda.Add(new StatusUsuarioPontoDemanda { StatusCadastro = StatusCadastro.UsuarioConvidado });
                integrante.Usuario = usuario;
            }
            else
            {
                usuario.StatusUsuarioPontoDemanda.Add(new StatusUsuarioPontoDemanda { StatusCadastro = StatusCadastro.EtapaDeInformacoesPessoaisCompleta });
                integrante = new Integrante(usuario);
                usuario.MapIntegrantes = new Collection<Integrante> { integrante };
                usuario.Integrante.Persona = _appPersona.Obter(usuario.ObterIdade(), usuario.Sexo, "adulto");
            }

            _repositorio.Criar(usuario);
            _repositorio.Salvar();
            return usuario;
        }

        public Usuario ValidarLogin(string email, string senha)
        {
            var usuario = _repositorio.ObterPorEmail(email);
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



