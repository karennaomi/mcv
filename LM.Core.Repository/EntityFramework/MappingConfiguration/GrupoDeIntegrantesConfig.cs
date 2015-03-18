using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.Repository.EntityFramework.MappingConfiguration
{
    public class GrupoDeIntegrantesConfig : EntityTypeConfiguration<GrupoDeIntegrantes>
    {
        public GrupoDeIntegrantesConfig()
        {
            ToTable("TB_GRUPO_INTEGRANTE");
            HasKey(g => g.Id);
            Property(g => g.Id).HasColumnName("ID_GRUPO_INTEGRANTE");
            Property(g => g.Nome).HasColumnName("NM_GRUPO_INTEGRANTE");

            HasMany(g => g.Integrantes).WithRequired(i => i.GrupoDeIntegrantes);
            HasMany(g => g.PontosDemanda).WithRequired(d => d.GrupoDeIntegrantes);
        }
    }
}
