using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.Repository.EntityFramework.MappingConfiguration
{
    public class ListaItemConfig : EntityTypeConfiguration<ListaItem>
    {
        public ListaItemConfig()
        {
            ToTable("TB_LISTA_PRODUTO_ITEM");
            HasKey(i => i.Id);
            Property(i => i.Id).HasColumnName("ID_LISTA_PRODUTO_ITEM");
            Property(i => i.QuantidadeDeConsumo).HasColumnName("QT_CONSUMO");
            Property(i => i.QuantidadeEmEstoque).HasColumnName("QT_ESTOQUE");
            Property(i => i.QuantidadeDoEstoqueEstimado).HasColumnName("QT_ESTIMADA_ESTOQUE").IsOptional();
            Property(i => i.QuantidadeDeSugestaoDeCompra).HasColumnName("QT_SUGESTAO_COMPRA").IsOptional();
            Property(i => i.Status).HasColumnName("TX_STATUS_ITEM");
            Property(i => i.ValorMedioDeConsumoPorIntegrante).HasColumnName("VL_CONSUMO_MEDIO_INTEGRANTE");
            Property(i => i.DataInclusao).HasColumnName("DT_INC").IsOptional();
            Property(i => i.DataAlteracao).HasColumnName("DT_ALT").IsOptional();

            HasRequired(i => i.Produto).WithMany().Map(m => m.MapKey("ID_PRODUTO"));
            HasRequired(i => i.Periodo).WithMany().Map(m => m.MapKey("ID_PERIODO_CONSUMO"));
        }
    }
}
