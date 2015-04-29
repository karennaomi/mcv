using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class LegacyTokenConfig : EntityTypeConfiguration<LegacyToken>
    {
        public LegacyTokenConfig()
        {
            ToTable("TB_Token_Smv");
            HasKey(g => g.Id);
            Property(g => g.Id).HasColumnName("TOKEN_SMV");
            Property(g => g.DataInicio).HasColumnName("DT_INICIO");
            Property(g => g.DataValidade).HasColumnName("DT_VALIDADE");
            Property(g => g.UsuarioId).HasColumnName("ID_USUARIO");
        }
    }
}
