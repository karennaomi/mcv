using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class ImagemConfig : EntityTypeConfiguration<Imagem>
    {
        public ImagemConfig()
        {
            ToTable("TB_IMAGEM");
            HasKey(g => g.Id);
            Property(g => g.Id).HasColumnName("ID_IMAGEM");
            Property(g => g.Path).HasColumnName("TX_URL_IMAGEM");
            Property(g => g.Interface).HasColumnName("ID_IMAGEM_INTERFACE");
            Property(g => g.Resolucao).HasColumnName("ID_IMAGEM_RESOLUCAO");
        }
    }
}
