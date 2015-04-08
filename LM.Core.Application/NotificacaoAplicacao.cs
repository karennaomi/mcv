using System;
using LM.Core.Domain;
using Ninject;
using System.Linq;

namespace LM.Core.Application
{
    public interface INotificacaoAplicacao
    {
        void NotificarIntegrantesDoPontoDamanda(long usuarioId, long pontoDemandaId, TipoTemplateMensagem tipoTemplate, string action);
    }

    public class NotificacaoAplicacao : INotificacaoAplicacao
    {
        private readonly IRestService _pushRestService;
        private readonly IPontoDemandaAplicacao _appPontoDemanda;
        private readonly ITemplateMensagemAplicacao _appTemplateMensagem;
        public NotificacaoAplicacao([Named("PushService")]IRestService pushRestService, IPontoDemandaAplicacao appPontoDemanda, ITemplateMensagemAplicacao appTemplateMensagem)
        {
            _pushRestService = pushRestService;
            _appPontoDemanda = appPontoDemanda;
            _appTemplateMensagem = appTemplateMensagem;
        }

        public void NotificarIntegrantesDoPontoDamanda(long usuarioId, long pontoDemandaId, TipoTemplateMensagem tipoTemplate, string action)
        {
            var pontoDemanda = _appPontoDemanda.Obter(usuarioId, pontoDemandaId);
            
            var integrantesComUsuario = pontoDemanda.GrupoDeIntegrantes.Integrantes.Where(i => i.Usuario != null).ToList();
            if (!integrantesComUsuario.Any()) return;
           
            var remetente = integrantesComUsuario.First(i => i.Usuario.Id == usuarioId).Usuario;
            
            var destinatarios = integrantesComUsuario.Where(i => i.Usuario.Id != usuarioId).Select(i => i.Usuario);
            var mensagem = _appTemplateMensagem.ObterPorTipo<TemplateMensagemPush>(tipoTemplate).Mensagem;
            foreach (var destinatario in destinatarios)
            {
                mensagem = TemplateProcessor.ProcessTemplate(mensagem, new { PontoDemanda = pontoDemanda, Remetente = remetente, Destinatario = destinatario });
                EnviarNotificacaoPush(destinatario.DeviceType, destinatario.DeviceId, mensagem, action);
            }
        }

        private void EnviarNotificacaoPush(string deviceType, string deviceId, string message, string action)
        {
            var content = new
            {
                DeviceId = deviceId,
                DeviceType = deviceType,
                Message = message,
                Action = action
            };
            _pushRestService.Post("sendpushmessage", content);
        }
    }
}
