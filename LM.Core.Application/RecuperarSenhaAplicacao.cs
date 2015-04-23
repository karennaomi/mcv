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
        private readonly ITemplateMensagemAplicacao _appTemplateMensagem;
        private readonly IFilaItemAplicacao _filaItemAplicacao;
        public RecuperarSenhaAplicacao(IRepositorioRecuperarSenha repositorio, IUsuarioAplicacao appUsuario, ITemplateMensagemAplicacao appTemplateMensagem, IFilaItemAplicacao filaItemAplicacao)
        {
            _repositorio = repositorio;
            _appUsuario = appUsuario;
            _appTemplateMensagem = appTemplateMensagem;
            _filaItemAplicacao = filaItemAplicacao;
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
            var template = _appTemplateMensagem.ObterPorTipo<TemplateMensagemEmail>(TipoTemplateMensagem.RecuperarSenha);
            var entity = new { Url = trocarSenhaUrl, recuperarSenha.Token, recuperarSenha.Usuario };
            var assunto = TemplateProcessor.ProcessTemplate(template.Assunto, entity);
            var corpo = TemplateProcessor.ProcessTemplate(template.Mensagem, entity);
            var filaItem = new FilaItem();
            filaItem.AdicionarMensagem(new FilaMensagemEmail
            {
                Assunto = assunto,
                Corpo = corpo,
                EmailDestinatario = recuperarSenha.Usuario.Email
            });
            _filaItemAplicacao.Criar(filaItem);
        }
    }
}
