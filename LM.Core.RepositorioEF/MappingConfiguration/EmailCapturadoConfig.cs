using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class EmailCapturadoConfig : EntityTypeConfiguration<EmailCapturado>
    {
        public EmailCapturadoConfig()
        {
            ToTable("TB_Email_Capturado");
            HasKey(g => g.Id);
            Property(g => g.Id).HasColumnName("ID_CAPTURED_EMAIL");
            Property(g => g.Email).HasColumnName("TX_EMAIL");
            Property(g => g.DataInclusao).HasColumnName("DT_INC");
        }
    }
}
