using System.Collections.Generic;
using LM.Core.Domain;
using Ninject;
using System.Linq;

namespace LM.Core.Application
{
    public interface INotificacaoAplicacao
    {
        void NotificarIntegrantesDoPontoDamanda(Usuario remetente, PontoDemanda pontoDemanda, TipoTemplateMensagem tipoTemplate, string action);
        void Notificar(Usuario remetente, Usuario destinatario, PontoDemanda pontoDemanda, TipoTemplateMensagem tipoTemplate, string action);
    }

    public class NotificacaoAplicacao : INotificacaoAplicacao
    {
        private readonly IRestService _pushRestService;
        private readonly ITemplateMensagemAplicacao _appTemplateMensagem;
        public NotificacaoAplicacao([Named("PushService")]IRestService pushRestService, ITemplateMensagemAplicacao appTemplateMensagem)
        {
            _pushRestService = pushRestService;
            _appTemplateMensagem = appTemplateMensagem;
        }

        public void Notificar(Usuario remetente, Usuario destinatario, PontoDemanda pontoDemanda, TipoTemplateMensagem tipoTemplate, string action)
        {
            var destinatarios = new List<Usuario> { destinatario };
            CriarMensagemEEnviar(remetente, pontoDemanda, tipoTemplate, action, destinatarios);
        }
        
        public void NotificarIntegrantesDoPontoDamanda(Usuario remetente, PontoDemanda pontoDemanda, TipoTemplateMensagem tipoTemplate, string action)
        {
            var integrantesComUsuario = pontoDemanda.GrupoDeIntegrantes.Integrantes.Where(i => i.Usuario != null).ToList();
            var destinatarios = integrantesComUsuario.Where(i => i.Usuario.Id != remetente.Id).Select(i => i.Usuario);
            CriarMensagemEEnviar(remetente, pontoDemanda, tipoTemplate, action, destinatarios);
        }

        private void CriarMensagemEEnviar(Usuario remetente, PontoDemanda pontoDemanda, TipoTemplateMensagem tipoTemplate, string action, IEnumerable<Usuario> destinatarios)
        {
            var mensagem = _appTemplateMensagem.ObterPorTipo<TemplateMensagemPush>(tipoTemplate).Mensagem;
            foreach (var destinatario in destinatarios)
            {
                mensagem = TemplateProcessor.ProcessTemplate(mensagem, new {PontoDemanda = pontoDemanda, Remetente = remetente, Destinatario = destinatario});
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
