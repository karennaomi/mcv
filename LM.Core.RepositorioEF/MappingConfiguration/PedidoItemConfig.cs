namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class PedidoItemConfig : EntityTypeConfiguration<PedidoItem>
    {
        public PedidoItemConfig()
        {
            ToTable("TB_PEDIDO");
            HasKey(p => p.Id);
            Property(p => p.Id).HasColumnName("ID_PEDIDO");
            Property(p => p.Status).HasColumnName("ID_STATUS_PEDIDO");
            Property(p => p.Data).HasColumnName("DT_PEDIDO").HasColumnType("smalldatetime");
            Property(p => p.Quantidade).HasColumnName("QT_SOLICITADA");
            Property(p => p.DataInclusao).HasColumnName("DT_INC").IsOptional().HasColumnType("smalldatetime");
            Property(p => p.DataAlteracao).HasColumnName("DT_ALT").IsOptional().HasColumnType("smalldatetime");

            HasRequired(p => p.Produto).WithMany().Map(m => m.MapKey("ID_PRODUTO"));
            HasRequired(p => p.PontoDemanda).WithMany().Map(m => m.MapKey("ID_PONTO_REAL_DEMANDA"));
            HasRequired(p => p.Integrante).WithMany().Map(m => m.MapKey("ID_INTEGRANTE"));
        }
    }
}
