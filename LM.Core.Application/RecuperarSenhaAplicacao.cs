using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.Application
{
    public interface IRecuperarSenhaAplicacao
    {
        RecuperarSenha RecuperarSenha(string email);
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

        public RecuperarSenha RecuperarSenha(string email)
        {
            var usuario = _appUsuario.Obter(email);
            var recuperarSenha = new RecuperarSenha {Usuario = new Usuario {Id = usuario.Id}};
            _repositorio.Criar(recuperarSenha);
            EnviarEmail(recuperarSenha);
            return recuperarSenha;
        }

        private void EnviarEmail(RecuperarSenha recuperarSenha)
        {
            var template = _appTemplateMensagem.ObterPorTipo<TemplateMensagemEmail>(TipoTemplateMensagem.RecuperarSenha);
            var entity = new { Url = "http://teste.com/recuperarsenha", recuperarSenha.Token, recuperarSenha.Usuario };
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
