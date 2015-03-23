using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class IntegranteConfig : EntityTypeConfiguration<Integrante>
    {
        public IntegranteConfig()
        {
            ToTable("TB_INTEGRANTE");
            HasKey(i => i.Id);
            Property(i => i.Id).HasColumnName("ID_INTEGRANTE");
            Property(i => i.Nome).HasColumnName("NM_INTEGRANTE");
            Property(i => i.DataNascimento).HasColumnName("DT_NASCIMENTO").IsOptional();
            Property(i => i.EmailConvite).HasColumnName("TX_EMAIL_CONVITE");
            Property(i => i.EhUsuarioSistema).HasColumnName("FL_USUARIO_SISTEMA");
            Property(i => i.EhUsuarioConvidado).HasColumnName("FL_USUARIO_CONVIDADO");
            Property(i => i.DataConvite).HasColumnName("DT_CONVITE").IsOptional();
            Property(i => i.DataInclusao).HasColumnName("DT_INC").IsOptional();
            Property(i => i.DataAlteracao).HasColumnName("DT_ALT").IsOptional();
            Property(i => i.Ativo).HasColumnName("FL_ATIVO");
            Property(i => i.Telefone).HasColumnName("TX_NUMERO_TELEFONE");
            Property(i => i.DDD).HasColumnName("NR_DDD");
            Property(i => i.Papel).HasColumnName("ID_INTEGRANTE_PAPEL");

            HasOptional(i => i.Usuario).WithMany(u => u.MapIntegrantes).Map(m => m.MapKey("ID_USUARIO"));
            HasRequired(i => i.GrupoDeIntegrantes).WithMany().Map(m => m.MapKey("ID_GRUPO_INTEGRANTE"));
            HasOptional(i => i.Persona).WithMany().Map(m => m.MapKey("ID_PERSONAS"));
        }
    }
}
