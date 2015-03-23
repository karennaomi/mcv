using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class CompraItemConfig : EntityTypeConfiguration<CompraItem>
    {
        public CompraItemConfig()
        {
            ToTable("TB_Compra_Item");
            HasKey(i => i.Id);
            Property(i => i.Id).HasColumnName("ID_COMPRA_ITEM");
            Property(i => i.Status).HasColumnName("ID_STATUS_COMPRA");
            Property(i => i.DataCompra).HasColumnName("DT_COMPRA").IsOptional();
            Property(i => i.Quantidade).HasColumnName("QT_COMPRA").IsOptional();
            Property(i => i.Valor).HasColumnName("VL_PRECO_ITEM").IsOptional();
            Property(i => i.DataInclusao).HasColumnName("DT_INC").IsOptional();
            Property(i => i.DataAlteracao).HasColumnName("DT_ALT").IsOptional();

            HasRequired(i => i.Compra).WithMany(c => c.Itens).Map(m => m.MapKey("ID_COMPRA"));
            HasOptional(i => i.ListaItem).WithMany().Map(m => m.MapKey("ID_LISTA_PRODUTO_ITEM"));
            HasOptional(i => i.Produto).WithMany().Map(m => m.MapKey("ID_PRODUTO"));
        }
    }

}
