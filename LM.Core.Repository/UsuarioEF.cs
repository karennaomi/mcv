﻿using System;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Repository.EntityFramework;
using System.Data.SqlClient;
using System.Linq;

namespace LM.Core.Repository
{
    public interface IRepositorioUsuario
    {
        Usuario Obter(long id);
        Usuario ObterPorEmail(string email);
        Usuario Criar(Usuario usuario);
        void AtualizarStatusCadastro(long usuarioId, StatusCadastro statusCadastro, long? pontoDemandaId = null);
        bool VerificarSeCpfJaExiste(string cpf);
        Usuario ValidarLogin(string email, string senha);
    }

    public class UsuarioEF : IRepositorioUsuario
    {
        private readonly ContextoEF _contextoEF;

        public UsuarioEF()
        {
            _contextoEF = new ContextoEF();
        }

        public Usuario Obter(long id)
        {
            return _contextoEF.Usuarios.Find(id);
        }

        public Usuario ObterPorEmail(string email)
        {
            return _contextoEF.Usuarios.SingleOrDefault(u => u.Email == email);
        }

        public Usuario Criar(Usuario usuario)
        {
            usuario = _contextoEF.Usuarios.Add(usuario);
            _contextoEF.SaveChanges();
            return usuario;
        }

        public void AtualizarStatusCadastro(long usuarioId, StatusCadastro statusCadastro, long? pontoDemandaId = null)
        {
            var usuario = Obter(usuarioId);
            usuario.StatusUsuarioPontoDemanda.StatusCadastro = statusCadastro;
            usuario.StatusUsuarioPontoDemanda.DataAlteracao = DateTime.Now;
            if (pontoDemandaId != null && pontoDemandaId > 0) usuario.StatusUsuarioPontoDemanda.PontoDemandaId = pontoDemandaId;
            _contextoEF.SaveChanges();
        }        

        public bool VerificarSeCpfJaExiste(string cpf)
        {
            return _contextoEF.Usuarios.Any(u => u.Cpf == cpf);
        }

        public Usuario ValidarLogin(string email, string senha)
        {
            var usuario = _contextoEF.Usuarios.SingleOrDefault(u => u.Login == email && u.Senha == senha);
            if(usuario == null) throw new LoginInvalidoException();
            return usuario;
        }
    }
}
