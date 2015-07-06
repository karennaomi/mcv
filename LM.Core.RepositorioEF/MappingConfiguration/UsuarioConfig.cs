using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class UsuarioConfig : EntityTypeConfiguration<Usuario>
    {
        public UsuarioConfig()
        {
            ToTable("TB_USUARIO");
            HasKey(u => u.Id);
            Property(u => u.Id).HasColumnName("ID_USUARIO");
            Property(u => u.Login).HasColumnName("TX_LOGIN");
            Property(u => u.Senha).HasColumnName("TX_SENHA");
            Property(i => i.DataInclusao).HasColumnName("DT_INC").IsOptional();
            Property(i => i.DataAlteracao).HasColumnName("DT_ALT").IsOptional();
            Property(u => u.DeviceId).HasColumnName("TX_DeviceId");
            Property(u => u.DeviceType).HasColumnName("TX_DeviceType");
            Property(u => u.Ativo).HasColumnName("FL_ATIVO");

            HasMany(u => u.StatusUsuarioPontoDemanda).WithRequired(s => s.Usuario).Map(m => m.MapKey("ID_USUARIO"));
            HasMany(u => u.Contratos).WithMany().Map(m => m.MapLeftKey("ID_USUARIO").MapRightKey("ID_CONTRATO").ToTable("TB_Contrato_Usuario"));
            HasOptional(u => u.Integrante).WithOptionalPrincipal(i => i.Usuario).Map(m => m.MapKey("ID_USUARIO"));
        }
    }
}
