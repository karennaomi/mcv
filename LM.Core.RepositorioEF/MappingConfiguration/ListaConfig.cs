namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class ListaConfig : EntityTypeConfiguration<Lista>
    {
        public ListaConfig()
        {
            ToTable("TB_LISTA_PRODUTO");
            HasKey(l => l.Id);
            Property(l => l.Id).HasColumnName("ID_LISTA_PRODUTO");
            Property(l => l.Nome).HasColumnName("NM_LISTA");
            Property(l => l.Status).HasColumnName("TX_STATUS_LISTA");
            Property(l => l.DataInclusao).HasColumnName("DT_INC").IsOptional();
            Property(l => l.DataAlteracao).HasColumnName("DT_ALT").IsOptional();
            
            HasMany(l => l.Itens).WithRequired(i => i.Lista).Map(m => m.MapKey("ID_LISTA_PRODUTO"));
        }
    }
}
