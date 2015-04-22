using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class FilaItemConfig : EntityTypeConfiguration<FilaItem>
    {
        public FilaItemConfig()
        {
            ToTable("TB_Fila");
            HasKey(g => g.Id);
            Property(g => g.Id).HasColumnName("ID_FILA");
            Property(g => g.TipoOperacaoId).HasColumnName("ID_TIPO_OPERACAO");
            Property(g => g.TipoServicoId).HasColumnName("ID_TIPO_SERVICO");
            Property(g => g.OrdemExecucao).HasColumnName("FL_ORDEM_EXECUCAO");
            Property(g => g.StatusFila).HasColumnName("FL_STATUS_FILA");
            Property(g => g.StatusProcessamento).HasColumnName("FL_STATUS_PROCESSAMENTO");
            Property(g => g.Origem).HasColumnName("TX_FILA_ORIGEM");
            Property(g => g.DataInclusao).HasColumnName("DT_INC").IsOptional();
            Property(g => g.DataAlteracao).HasColumnName("DT_ALT").IsOptional();

            //HasMany(g => g.Mensagens).WithRequired(i => i.Fila).Map(m => m.MapKey("ID_FILA"));
        }
    }
}
