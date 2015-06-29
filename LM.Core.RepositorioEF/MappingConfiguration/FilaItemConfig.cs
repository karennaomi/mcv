using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class FilaItemConfig : EntityTypeConfiguration<FilaItem>
    {
        public FilaItemConfig()
        {
            Map<FilaItemMensagem>(m => m.Requires("ID_TIPO_OPERACAO").HasValue((int)TipoFilaItem.Mensagem));
            Map<FilaItemProduto>(m => m.Requires("ID_TIPO_OPERACAO").HasValue((int)TipoFilaItem.Produto));
            
            ToTable("TB_Fila");
            HasKey(g => g.Id);
            Property(g => g.Id).HasColumnName("ID_FILA");
            Property(g => g.TipoServicoId).HasColumnName("ID_TIPO_SERVICO");
            Property(g => g.OrdemExecucao).HasColumnName("FL_ORDEM_EXECUCAO").IsOptional();
            Property(g => g.StatusFila).HasColumnName("FL_STATUS_FILA");
            Property(g => g.StatusProcessamento).HasColumnName("FL_STATUS_PROCESSAMENTO");
            Property(g => g.Origem).HasColumnName("TX_FILA_ORIGEM");
            Property(g => g.DataInclusao).HasColumnName("DT_INC").IsOptional();
            Property(g => g.DataAlteracao).HasColumnName("DT_ALT").IsOptional();
        }
    }

    public class FilaItemMensagemConfig : EntityTypeConfiguration<FilaItemMensagem>
    {
        public FilaItemMensagemConfig()
        {
            HasMany(f => f.FilaMensagens).WithRequired(m => m.FilaItem).Map(m => m.MapKey("ID_FILA"));
        }
    }

    public class FilaItemProdutoConfig : EntityTypeConfiguration<FilaItemProduto>
    {
        public FilaItemProdutoConfig()
        {
            HasMany(f => f.FilaProdutos).WithRequired(p => p.FilaItem).Map(m => m.MapKey("ID_FILA"));
        }
    }
}
