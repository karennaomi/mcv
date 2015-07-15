using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class ProdutoPrecoConfig : EntityTypeConfiguration<ProdutoPreco>
    {
        public ProdutoPrecoConfig()
        {
            ToTable("TB_Produto_Preco");
            HasKey(g => g.Id);
            Property(g => g.Id).HasColumnName("ID_PRODUTO_PRECO");
            Property(g => g.PrecoMin).HasColumnName("VL_PRECO_MIN_PRODUTO").IsOptional();
            Property(g => g.PrecoMax).HasColumnName("VL_PRECO_MAX_PRODUTO").IsOptional();
            Property(p => p.DataPreco).HasColumnName("DT_PRECO_PRODUTO").IsOptional();
            Property(p => p.Ativo).HasColumnName("FL_ATIVO").IsOptional();
            Property(p => p.DataInclusao).HasColumnName("DT_INC").IsOptional();
            Property(p => p.DataAlteracao).HasColumnName("DT_ALT").IsOptional();
        }
    }
}
