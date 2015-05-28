using LM.Core.Domain;
using LM.Core.Domain.Servicos;
using Ninject;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.Application
{
    public interface INotificacaoAplicacao
    {
        void NotificarIntegrantesDoPontoDamanda(Integrante remetente, PontoDemanda pontoDemanda, TipoTemplateMensagem tipoTemplate, object extraParams);
        void Notificar(Integrante remetente, Integrante destinatario, PontoDemanda pontoDemanda, TipoTemplateMensagem tipoTemplate, object extraParams);
    }

    public class NotificacaoAplicacao : INotificacaoAplicacao
    {
        private readonly IServicoRest _pushRestService;
        private readonly ITemplateMensagemAplicacao _appTemplateMensagem;
        private readonly IFilaItemAplicacao _filaItemAplicacao;
        public NotificacaoAplicacao([Named("PushService")]IServicoRest pushRestService, ITemplateMensagemAplicacao appTemplateMensagem, IFilaItemAplicacao filaItemAplicacao)
        {
            _pushRestService = pushRestService;
            _appTemplateMensagem = appTemplateMensagem;
            _filaItemAplicacao = filaItemAplicacao;
        }

        public void Notificar(Integrante remetente, Integrante destinatario, PontoDemanda pontoDemanda, TipoTemplateMensagem tipoTemplate, object extraParams)
        {
            var destinatarios = new List<Integrante> { destinatario };
            CriarMensagemEEnviar(remetente, pontoDemanda, tipoTemplate, extraParams, destinatarios);
        }

        public void NotificarIntegrantesDoPontoDamanda(Integrante remetente, PontoDemanda pontoDemanda, TipoTemplateMensagem tipoTemplate, object extraParams)
        {
            var integrantesComUsuario = pontoDemanda.GruposDeIntegrantes.Where(g => g.Integrante.Usuario != null).Select(g=> g.Integrante).ToList();
            var destinatarios = integrantesComUsuario.Where(i => i.Id != remetente.Id);
            CriarMensagemEEnviar(remetente, pontoDemanda, tipoTemplate, extraParams, destinatarios);
        }

        private void CriarMensagemEEnviar(Integrante remetente, PontoDemanda pontoDemanda, TipoTemplateMensagem tipoTemplate, object extraParams, IEnumerable<Integrante> destinatarios)
        {
            var templateMensagem = _appTemplateMensagem.ObterPorTipoTemplate(tipoTemplate);
            foreach (var destinatario in destinatarios)
            {
                var entity = new { PontoDemanda = pontoDemanda, Remetente = remetente, Destinatario = destinatario, Extra = extraParams };
                if(templateMensagem is TemplateMensagemPush)
                {
                    var templateMensagemPush = templateMensagem as TemplateMensagemPush;
                    var mensagem = TemplateProcessor.ProcessTemplate(templateMensagemPush.Mensagem, entity);
                    var action = extraParams.GetType().GetProperty("Action").GetValue(extraParams).ToString();
                    EnviarNotificacaoPush(destinatario.Usuario.DeviceType, destinatario.Usuario.DeviceId, mensagem, action);
                } 
                else if (templateMensagem is TemplateMensagemEmail)
                {
                    var templateMensagemEmail = templateMensagem as TemplateMensagemEmail;
                    var assunto = TemplateProcessor.ProcessTemplate(templateMensagemEmail.Assunto, entity);
                    var corpo = TemplateProcessor.ProcessTemplate(templateMensagemEmail.Mensagem, entity);
                    EnviarEmail(assunto, corpo, entity.Destinatario.Email);
                }
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

        private void EnviarEmail(string assunto, string corpo, string emailDestinatario)
        {
            var filaItem = new FilaItem();
            filaItem.AdicionarMensagem(new FilaMensagemEmail
            {
                Assunto = assunto,
                Corpo = corpo,
                EmailDestinatario = emailDestinatario
            });
            _filaItemAplicacao.Criar(filaItem);
        }
    }
}
