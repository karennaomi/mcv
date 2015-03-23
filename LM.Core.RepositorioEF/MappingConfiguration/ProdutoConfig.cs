using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class ProdutoConfig : EntityTypeConfiguration<Produto>
    {
        public ProdutoConfig()
        {
            ToTable("TB_PRODUTO");
            HasKey(g => g.Id);
            Property(g => g.Id).HasColumnName("ID_PRODUTO");

            HasRequired(p => p.Info).WithRequiredPrincipal();
            HasMany(g => g.Imagens).WithMany().Map(m => m.ToTable("TB_PRODUTO_IMAGEM").MapLeftKey("ID_PRODUTO").MapRightKey("ID_IMAGEM"));
            HasMany(g => g.Categorias).WithMany().Map(m => m.ToTable("TB_PRODUTO_CATEGORIA").MapLeftKey("ID_PRODUTO").MapRightKey("ID_CATEGORIA"));

        }
    }

    public class ProdutoInfoConfig : EntityTypeConfiguration<ProdutoInfo>
    {
        public ProdutoInfoConfig()
        {
            ToTable("TB_PRODUTO_MASTER_DATA");
            HasKey(g => g.Id);
            Property(g => g.Id).HasColumnName("ID_PRODUTO");
            Property(g => g.Nome).HasColumnName("NM_PRODUTO_COMPLETO");
            Property(g => g.Marca).HasColumnName("TX_MARCA_PRODUTO");
        }
    }
}
