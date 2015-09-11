using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class PlanoConfig : EntityTypeConfiguration<Plano>
    {
        public PlanoConfig()
        {
            ToTable("TB_PLANO");
            HasKey(g => g.Id);
            Property(g => g.Id).HasColumnName("ID_PLANO");
            Property(g => g.Nome).HasColumnName("NM_PLANO");
            Property(g => g.Periodo).HasColumnName("VL_PERIODO");
            Property(g => g.Valor).HasColumnName("VL_VALOR");
            Property(g => g.Chamada).HasColumnName("TX_CHAMADA");
            Property(g => g.DataInclusao).HasColumnName("DT_INC");
            Property(g => g.DataAlteracao).HasColumnName("DT_ALT");
            Property(g => g.Ativo).HasColumnName("FL_ATIVO");
        }
    }
}
