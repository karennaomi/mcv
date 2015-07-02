using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class FilaProdutoConfig : EntityTypeConfiguration<FilaProduto>
    {
        public FilaProdutoConfig()
        {
            ToTable("TB_Fila_Produto");
            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("ID_FILA_PRODUTO");
            Property(p => p.ProdutoId).HasColumnName("ID_PRODUTO");
            Property(p => p.Ean).HasColumnName("CD_PRODUTO_EAN");
            Property(p => p.Descricao).HasColumnName("TX_DESCRICAO_PRODUTO");
            Property(p => p.Imagem).HasColumnName("TX_PRODUTO_IMAGEM_URL");
            Property(p => p.DataInclusao).HasColumnName("DT_INC");
        }
    }
}