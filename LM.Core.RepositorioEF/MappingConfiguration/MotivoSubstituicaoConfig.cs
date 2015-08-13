using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class MotivoSubstituicaoConfig : EntityTypeConfiguration<MotivoSubstituicao>
    {
        public MotivoSubstituicaoConfig()
        {
            ToTable("TB_Motivo_Substituicao");
            HasKey(g => g.Id);
            Property(g => g.Id).HasColumnName("ID_MOTIVO");
            Property(g => g.Motivo).HasColumnName("NM_MOTIVO");
            Property(g => g.Ativo).HasColumnName("FL_ATIVO");
        }
    }
}
