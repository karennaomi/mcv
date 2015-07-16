
using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class ContatoConfig : EntityTypeConfiguration<Contato>
    {
        public ContatoConfig()
        {
            ToTable("TB_Contato");
            HasKey(g => g.Id);
            Property(g => g.Id).HasColumnName("ID_CONTATO");
            Property(g => g.Nome).HasColumnName("TX_NOME");
            Property(g => g.Email).HasColumnName("TX_EMAIL");
            Property(g => g.Mensagem).HasColumnName("TX_MENSAGEM");
            Property(g => g.DataInclusao).HasColumnName("DT_INC");
        }
    }
}
