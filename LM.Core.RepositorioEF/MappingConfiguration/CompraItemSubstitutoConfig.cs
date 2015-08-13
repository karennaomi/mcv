using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class CompraItemSubstitutoConfig : EntityTypeConfiguration<CompraItemSubstituto>
    {
        public CompraItemSubstitutoConfig()
        {
            ToTable("TB_Compra_Item_Substituto");
            HasKey(i => i.SubstitutoId);
            Property(i => i.SubstitutoId).HasColumnName("ID_COMPRA_ITEM_SUBSTITUTO");
            Property(i => i.DataInclusao).HasColumnName("DT_INC").IsOptional();

            HasRequired(i => i.Motivo).WithMany().Map(m => m.MapKey("ID_MOTIVO"));
            HasRequired(i => i.Original).WithOptional().Map(m => m.MapKey("ID_COMPRA_ITEM_PRINCIPAL"));
        }
    }
}
