using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.Repository.EntityFramework.MappingConfiguration
{
    public class ImagemConfig : EntityTypeConfiguration<Imagem>
    {
        public ImagemConfig()
        {
            ToTable("TB_IMAGEM");
            HasKey(g => g.Id);
            Property(g => g.Id).HasColumnName("ID_IMAGEM");
            Property(g => g.Path).HasColumnName("TX_URL_IMAGEM");
        }
    }
}
