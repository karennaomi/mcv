using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class FilaMensagemConfig : EntityTypeConfiguration<FilaMensagem>
    {
        public FilaMensagemConfig()
        {
            Map<FilaMensagemEmail>(m => m.Requires("ID_TIPO_MENSAGEM").HasValue((int)TipoMensagem.Email));
            Map<FilaMensagemSms>(m => m.Requires("ID_TIPO_MENSAGEM").HasValue((int)TipoMensagem.Sms));
            
            ToTable("TB_Fila_Mensagem");
            HasKey(m => m.Id);
            Property(m => m.Id).HasColumnName("ID_FILA_MENSAGEM");
            Property(m => m.DataInclusao).HasColumnName("DT_INC");
        }
    }

    public class FilaMensagemEmailConfig : EntityTypeConfiguration<FilaMensagemEmail>
    {
        public FilaMensagemEmailConfig()
        {
            Property(e => e.EmailDestinatario).HasColumnName("TX_EMAIL_DESTINO");
            Property(e => e.Assunto).HasColumnName("TX_ASSUNTO");
            Property(e => e.Corpo).HasColumnName("TX_CORPO");
        }
    }

    public class FilaMensagemSmsConfig : EntityTypeConfiguration<FilaMensagemSms>
    {
        public FilaMensagemSmsConfig()
        {
            Property(s => s.NumeroDestinatario).HasColumnName("TX_NUMERO_DESTINO");
            Property(s => s.Mensagem).HasColumnName("TX_MENSAGEM_SMS");
        }
    }
}
