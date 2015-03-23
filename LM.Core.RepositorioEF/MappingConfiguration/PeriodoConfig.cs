﻿namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class PeriodoConfig : EntityTypeConfiguration<Periodo>
    {
        public PeriodoConfig()
        {
            ToTable("TB_PERIODO_CONSUMO");
            HasKey(i => i.Id);
            Property(i => i.Id).HasColumnName("ID_PERIODO_CONSUMO");
            Property(i => i.Nome).HasColumnName("NM_PERIODO_CONSUMO");
            Property(i => i.FatorConversaoDia).HasColumnName("VL_FATOR_CONVERSAO_DIA");
        }
    }
}