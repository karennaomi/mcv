using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class TemplateMensagemConfig : EntityTypeConfiguration<TemplateMensagem>
    {
        public TemplateMensagemConfig()
        {
            ToTable("TB_Mensagem");
            HasKey(g => g.Id);
            Property(g => g.Id).HasColumnName("ID_MENSAGEM");
            Property(g => g.Mensagem).HasColumnName("TX_MENSAGEM");
            Property(g => g.Tipo).HasColumnName("ID_TIPO_TEMPLATE");
        }
    }

    public class TemplateMensagemPushConfig : EntityTypeConfiguration<TemplateMensagemPush>
    {
    }

    public class TemplateMensagemEmailConfig : EntityTypeConfiguration<TemplateMensagemEmail>
    {
        public TemplateMensagemEmailConfig()
        {
            Property(g => g.Assunto).HasColumnName("TX_ASSUNTO");
            Property(g => g.UrlHtml).HasColumnName("TX_URL_HTML");
        }
    }
}
