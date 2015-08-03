using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class LojaConfig : EntityTypeConfiguration<Loja>
    {
        public LojaConfig()
        {
            ToTable("TB_Loja");
            HasKey(l => l.Id);
            Property(l => l.Id).HasColumnName("ID_LOJA");
            Property(l => l.Nome).HasColumnName("NM_LOJA");
            Property(l => l.LocalizadorId).HasColumnName("ID_LOJA_LOCALIZADOR");
            Property(l => l.LocalizadorOrigem).HasColumnName("TX_ORIGEM");
            Property(l => l.DataInclusao).HasColumnName("DT_INC").IsOptional();
            Property(l => l.DataAlteracao).HasColumnName("DT_ALT").IsOptional();
            Ignore(l => l.Proximidade);

            HasRequired(p => p.Info).WithRequiredPrincipal();
        }
    }

    public class LojaInfoConfig : EntityTypeConfiguration<LojaInfo>
    {
        public LojaInfoConfig()
        {
            ToTable("TB_Loja_Master_Data");
            HasKey(l => l.Id);
            Property(l => l.Id).HasColumnName("ID_LOJA");
            Property(l => l.RazaoSocial).HasColumnName("NM_RAZAO_SOCIAL");
            Property(l => l.Telefone).HasColumnName("TX_TELEFONE_LOJA");
            Property(l => l.DataInclusao).HasColumnName("DT_INC").IsOptional();
            Property(l => l.DataAlteracao).HasColumnName("DT_ALT").IsOptional();

            HasRequired(g => g.Endereco).WithMany().Map(m => m.MapKey("ID_ENDERECO"));
        }
    }
}
