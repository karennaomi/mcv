using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class AnimalConfig : EntityTypeConfiguration<Animal>
    {
        public AnimalConfig()
        {
            ToTable("TB_Animal");
            HasKey(g => g.Id);
            Property(g => g.Id).HasColumnName("ID_ANIMAL");
            Property(g => g.Nome).HasColumnName("NM_ANIMAL");
            Property(g => g.Ativo).HasColumnName("FL_ATIVO");
        }
    }
}
