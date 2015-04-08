using System;
namespace LM.Core.Domain
{
    public abstract class TemplateMensagem
    {
        public int Id { get; set; }
        public string Mensagem { get; set; }
        public TipoTemplateMensagem Tipo { get; set; }
    }

    public class TemplateMensagemPush : TemplateMensagem
    {
    }

    public class TemplateMensagemEmail : TemplateMensagem
    {
        public string Assunto { get; set; }
        public string UrlHtml { get; set; }
    }
}
