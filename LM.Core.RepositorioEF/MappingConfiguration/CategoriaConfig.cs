using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class CategoriaConfig : EntityTypeConfiguration<Categoria>
    {
        public CategoriaConfig()
        {
            ToTable("TB_CATEGORIA");
            HasKey(g => g.Id);
            Property(g => g.Id).HasColumnName("ID_CATEGORIA");
            Property(g => g.Nome).HasColumnName("NM_CATEGORIA");
            Property(g => g.Cor).HasColumnName("NM_HEXA_COR");
            Property(g => g.Ativo).HasColumnName("FL_CATEGORIA_ATIVA");

            HasMany(g => g.SubCategorias).WithOptional(i => i.CategoriaPai).Map(m => m.MapKey("ID_CATEGORIA_PAI"));
        }
    }
}
