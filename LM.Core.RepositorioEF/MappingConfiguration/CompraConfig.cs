using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class CompraConfig : EntityTypeConfiguration<Compra>
    {
        public CompraConfig()
        {
            ToTable("TB_Compra");
            HasKey(c => c.Id);
            Property(c => c.Id).HasColumnName("ID_COMPRA");
            Property(c => c.LojaId).HasColumnName("ID_LOJA");
            Property(c => c.DataInicioCompra).HasColumnName("DT_INICIO_COMPRA").IsOptional();
            Property(c => c.DataFimCompra).HasColumnName("DT_FIM_COMPRA").IsOptional();
            Property(c => c.DataCapturaPrimeiroItemCompra).HasColumnName("DT_CAPTURA_PRIMEIRO_ITEM_COMPRA").IsOptional();
            Property(c => c.DataInclusao).HasColumnName("DT_INC").IsOptional();
            Property(c => c.DataAlteracao).HasColumnName("DT_ALT").IsOptional();

            HasRequired(c => c.PontoDemanda).WithMany().Map(m => m.MapKey("ID_PONTO_REAL_DEMANDA"));
            HasRequired(c => c.Integrante).WithMany().Map(m => m.MapKey("ID_INTEGRANTE"));
        }
    }
}
