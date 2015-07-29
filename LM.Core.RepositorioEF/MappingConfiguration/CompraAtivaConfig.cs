using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class CompraAtivaConfig : EntityTypeConfiguration<CompraAtiva>
    {
        public CompraAtivaConfig()
        {
            ToTable("TB_CompraAtiva");
            HasKey(c => c.Id);
            Property(c => c.Id).HasColumnName("ID_CompraAtiva");
            Property(c => c.InicioCompra).HasColumnName("DT_Inicio");
            Property(c => c.FimCompra).HasColumnName("DT_Fim").IsOptional();

            HasRequired(c => c.PontoDemanda).WithMany().Map(m => m.MapKey("ID_Ponto_Real_Demanda"));
            HasRequired(c => c.Usuario).WithMany().Map(m => m.MapKey("ID_Usuario")).WillCascadeOnDelete(false);
        }
    }
}
