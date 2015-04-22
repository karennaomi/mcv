using System;

namespace LM.Core.Domain
{
    public enum TipoMensagem
    {
        Email = 1,
        Sms = 2
    }

    public abstract class FilaMensagem
    {
        public long Id { get; set; }
        public DateTime? DataInclusao { get; set; }
     
        public virtual FilaItem Fila { get; set; }
    }

    public class FilaMensagemEmail : FilaMensagem
    {
        public string EmailDestinatario { get; set; }
        public string Assunto { get; set; }
        public string Corpo { get; set; }
    }

    public class FilaMensagemSms : FilaMensagem
    {
        public string NumeroDestinatario { get; set; }
        public string Mensagem { get; set; }
    }
}
