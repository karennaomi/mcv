using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class GrupoDeIntegrantesConfig : EntityTypeConfiguration<GrupoDeIntegrantes>
    {
        public GrupoDeIntegrantesConfig()
        {
            ToTable("TB_GRUPO_INTEGRANTE");
            HasKey(g => g.Id);
            Property(g => g.Id).HasColumnName("ID_GRUPO_INTEGRANTE");
            Property(g => g.Nome).HasColumnName("NM_GRUPO_INTEGRANTE");
            Property(g => g.Papel).HasColumnName("ID_PAPEL_INTEGRANTE");

            HasRequired(g => g.Integrante).WithMany(i => i.GruposDeIntegrantes).Map(m => m.MapKey("ID_INTEGRANTE"));
            HasRequired(g => g.PontoDemanda).WithMany(d => d.GruposDeIntegrantes).Map(m => m.MapKey("ID_PONTO_REAL_DEMANDA"));
        }
    }
}
