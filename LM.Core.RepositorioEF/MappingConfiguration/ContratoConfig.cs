
using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class ContratoConfig : EntityTypeConfiguration<Contrato>
    {
        public ContratoConfig()
        {
            ToTable("TB_Contrato");
            HasKey(c => c.Id);
            Property(c => c.Id).HasColumnName("ID_CONTRATO");
            Property(c => c.Nome).HasColumnName("NM_CONTRATO");
            Property(c => c.Numero).HasColumnName("NR_CONTRATO");
            Property(c => c.InicioVigencia).HasColumnName("DT_INI_VIGENCIA");
            Property(c => c.FimVigencia).HasColumnName("DT_FIM_VIGENCIA");
            Property(c => c.Conteudo).HasColumnName("TX_CONTEUDO");
            Property(c => c.Ativo).HasColumnName("FL_ATIVO");
            Property(c => c.DataInclusao).HasColumnName("DT_INC");
            Property(c => c.DataAlteracao).HasColumnName("DT_ALT").IsOptional();
        }
    }
}
