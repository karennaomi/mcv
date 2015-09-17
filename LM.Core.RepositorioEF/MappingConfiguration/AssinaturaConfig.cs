using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class AssinaturaConfig : EntityTypeConfiguration<Assinatura>
    {
        public AssinaturaConfig()
        {
            ToTable("TB_ASSINATURA");
            HasKey(a => a.Id);
            Property(a => a.Id).HasColumnName("ID_ASSINATURA");
            Property(a => a.DataInclusao).HasColumnName("DT_INC");
            Property(a => a.DataAlteracao).HasColumnName("DT_ALT");
            Property(a => a.Status).HasColumnName("FL_STATUS");

            HasRequired(a => a.Usuario).WithMany().Map(m => m.MapKey("ID_USUARIO"));
            HasRequired(a => a.Plano).WithMany().Map(m => m.MapKey("ID_PLANO"));
        }
    }
}
