using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class PersonaConfig : EntityTypeConfiguration<Persona>
    {
        public PersonaConfig()
        {
            ToTable("TB_PERSONAS");
            HasKey(i => i.Id);
            Property(i => i.Id).HasColumnName("ID_PERSONAS");
            Property(i => i.Perfil).HasColumnName("NM_PERSONAS_PERFIL");
            Property(i => i.Sexo).HasColumnName("TP_SEXO");
            Property(i => i.FaixaEtaria).HasColumnName("TX_FAIXA_ETARIA");
            Property(i => i.IdadeInicial).HasColumnName("VL_IDADE_INI");
            Property(i => i.IdadeFinal).HasColumnName("VL_IDADE_FIM");
            Property(i => i.Ordem).HasColumnName("VL_ORDEM");
        }
    }
}
