using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class FilaMensagemConfig : EntityTypeConfiguration<FilaMensagem>
    {
        public FilaMensagemConfig()
        {
            ToTable("TB_Fila_Mensagem");
            HasKey(g => g.Id);
            Property(g => g.Id).HasColumnName("ID_FILA_MENSAGEM");
            Property(g => g.DataInclusao).HasColumnName("DT_INC");

            HasRequired(g => g.Fila).WithMany(i => i.Mensagens).Map(m => m.MapKey("ID_FILA"));

            Map<FilaMensagemEmail>(m => m.Requires("ID_TIPO_MENSAGEM").HasValue((int)TipoMensagem.Email));
            Map<FilaMensagemSms>(m => m.Requires("ID_TIPO_MENSAGEM").HasValue((int)TipoMensagem.Sms));
        }
    }

    public class FilaMensagemEmailConfig : EntityTypeConfiguration<FilaMensagemEmail>
    {
        public FilaMensagemEmailConfig()
        {
            Property(g => g.EmailDestinatario).HasColumnName("TX_EMAIL_DESTINO");
            Property(g => g.Assunto).HasColumnName("TX_ASSUNTO");
            Property(g => g.Corpo).HasColumnName("TX_CORPO");
        }
    }

    public class FilaMensagemSmsConfig : EntityTypeConfiguration<FilaMensagemSms>
    {
        public FilaMensagemSmsConfig()
        {
            Property(g => g.NumeroDestinatario).HasColumnName("TX_NUMERO_DESTINO");
            Property(g => g.Mensagem).HasColumnName("TX_MENSAGEM_SMS");
        }
    }
}
