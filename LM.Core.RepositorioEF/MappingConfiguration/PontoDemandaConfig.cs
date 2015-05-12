using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class PontoDemandaConfig : EntityTypeConfiguration<PontoDemanda>
    {
        public PontoDemandaConfig()
        {
            ToTable("TB_PONTO_REAL_DEMANDA");
            HasKey(d => d.Id);
            Property(d => d.Id).HasColumnName("ID_PONTO_REAL_DEMANDA");
            Property(d => d.Nome).HasColumnName("NM_PONTO_REAL_DEMANDA");
            Property(d => d.QuantidadeDiasAlertaReposicao).HasColumnName("QT_DIAS_ALERTA_REPOSICAO").IsOptional();
            Property(d => d.QuantidadeDiasCoberturaEstoque).HasColumnName("QT_DIAS_COBERTURA_ESTOQUE").IsOptional();
            Property(d => d.DataInclusao).HasColumnName("DT_INC").IsOptional();
            Property(d => d.DataAlteracao).HasColumnName("DT_ALT").IsOptional();
            Property(d => d.Tipo).HasColumnName("ID_TIPO_PONTO_REAL_DEMANDA").IsOptional();

            HasRequired(u => u.UsuarioCriador).WithMany().Map(m => m.MapKey("ID_USUARIO_CRIADOR"));
            HasRequired(d => d.Endereco).WithMany().Map(m => m.MapKey("ID_ENDERECO"));
            HasRequired(d => d.GrupoDeIntegrantes).WithMany().Map(m => m.MapKey("ID_GRUPO_INTEGRANTE"));
            HasMany(d => d.Listas).WithRequired(l => l.PontoDemanda).Map(m => m.MapKey("ID_PONTO_REAL_DEMANDA"));
            HasMany(d => d.LojasFavoritas).WithMany().Map(m => m.ToTable("TB_Ponto_Real_Demanda_Loja").MapLeftKey("ID_PONTO_REAL_DEMANDA").MapRightKey("ID_LOJA"));
        }
    }
}
