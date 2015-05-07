using System;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using LM.Core.Domain.CustomException;

namespace LM.Core.Application
{
    public interface IRecuperarSenhaAplicacao
    {
        RecuperarSenha RecuperarSenha(string email, string trocarSenhaUrl);
        bool ValidarToken(Guid token);
        void TrocarSenha(Guid token, string novaSenha);
    }

    public class RecuperarSenhaAplicacao : IRecuperarSenhaAplicacao
    {
        private readonly IRepositorioRecuperarSenha _repositorio;
        private readonly IUsuarioAplicacao _appUsuario;
        private readonly INotificacaoAplicacao _appNotificacao;

        public RecuperarSenhaAplicacao(IRepositorioRecuperarSenha repositorio, IUsuarioAplicacao appUsuario, INotificacaoAplicacao appNotificacao)
        {
            _repositorio = repositorio;
            _appUsuario = appUsuario;
            _appNotificacao = appNotificacao;
        }

        public RecuperarSenha RecuperarSenha(string email, string trocarSenhaUrl)
        {
            var usuario = _appUsuario.Obter(email);
            var recuperarSenha = new RecuperarSenha {Usuario = new Usuario {Id = usuario.Id}};
            _repositorio.Criar(recuperarSenha);
            recuperarSenha.Usuario = usuario;
            EnviarEmail(recuperarSenha, trocarSenhaUrl);
            return recuperarSenha;
        }

        public bool ValidarToken(Guid token)
        {
            var recuperarSenha = _repositorio.ObterPorToken(token);
            return recuperarSenha != null && recuperarSenha.TokenValido();
        }

        public void TrocarSenha(Guid token, string novaSenha)
        {
            if(!ValidarToken(token)) throw new ApplicationException("Token está expirado.");
            var recuperarSenha = _repositorio.ObterPorToken(token);
            recuperarSenha.Usuario.Senha = PasswordHash.CreateHash(novaSenha);
            _repositorio.Salvar();
        }

        private void EnviarEmail(RecuperarSenha recuperarSenha, string trocarSenhaUrl)
        {
            var extraParams = new { Url = trocarSenhaUrl, recuperarSenha.Token };
            _appNotificacao.Notificar(null, recuperarSenha.Usuario.Integrante, null, TipoTemplateMensagem.RecuperarSenha, extraParams);
        }
    }
}
