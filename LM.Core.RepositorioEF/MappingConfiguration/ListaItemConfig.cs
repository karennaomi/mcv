using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class ListaItemConfig : EntityTypeConfiguration<ListaItem>
    {
        public ListaItemConfig()
        {
            ToTable("TB_LISTA_PRODUTO_ITEM");
            HasKey(i => i.Id);
            Property(i => i.Id).HasColumnName("ID_LISTA_PRODUTO_ITEM");
            Property(i => i.QuantidadeConsumo).HasColumnName("QT_CONSUMO").IsOptional();
            Property(i => i.QuantidadeEstoque).HasColumnName("QT_ESTOQUE").IsOptional();
            Property(i => i.QuantidadeDoEstoqueEstimado).HasColumnName("QT_ESTIMADA_ESTOQUE").IsOptional();
            Property(i => i.QuantidadeDeSugestaoDeCompra).HasColumnName("QT_SUGESTAO_COMPRA").IsOptional();
            Property(i => i.Status).HasColumnName("TX_STATUS_ITEM");
            Property(i => i.ValorMedioDeConsumoPorIntegrante).HasColumnName("VL_CONSUMO_MEDIO_INTEGRANTE").IsOptional();
            Property(i => i.DataInclusao).HasColumnName("DT_INC").IsOptional();
            Property(i => i.DataAlteracao).HasColumnName("DT_ALT").IsOptional();
            Property(i => i.EhSugestaoDeCompra).HasColumnName("FL_ALERTA_SUGESTAO_COMPRA");
            Property(i => i.EhEssencial).HasColumnName("FL_NBO");

            HasRequired(i => i.Produto).WithMany().Map(m => m.MapKey("ID_PRODUTO"));
            HasRequired(i => i.Periodo).WithMany().Map(m => m.MapKey("ID_PERIODO_CONSUMO"));
        }
    }
}
