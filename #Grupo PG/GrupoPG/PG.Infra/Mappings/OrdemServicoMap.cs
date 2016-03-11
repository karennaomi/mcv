using PG.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Infra.Mappings
{
    public class OrdemServicoMap : EntityTypeConfiguration<OrdemServico>
    {
        public OrdemServicoMap()
        {
            ToTable("TB_OS");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("IdOS");
            Property(x => x.DataEntrega).HasColumnName("DtEntrega");
            Property(x => x.DataFinalizacao).HasColumnName("DtFinalizacao");
            Property(x => x.Defeito).HasColumnName("TxDefeito");
            Property(x => x.DtChegada).HasColumnName("DtChegada");
            Property(x => x.DtChegadaTransportadora).HasColumnName("DtChegadaTransportadora");
            Property(x => x.Habilitada).HasColumnName("FlHabilitada");
            Property(x => x.HoraEntrega).HasColumnName("HrEntrega");
            Property(x => x.LocalizacaoMaquina).HasColumnName("TxLocalizacaoMaquina");
            Property(x => x.NomeContato).HasColumnName("NomeContato");
            Property(x => x.PaUnificado).HasColumnName("PaUnificado");
            Property(x => x.Serie).HasColumnName("TxSerie");
            Property(x => x.Sinalizacao).HasColumnName("TxSinalizacao");
            Property(x => x.TelefoneContato).HasColumnName("TelefoneContato");
            Property(x => x.ValorTotal).HasColumnName("VlrOrcamento");
            Property(u => u.TipoFixacaoID).HasColumnName("IdTipoFixacao");
            Property(u => u.TipoInstalacaoId).HasColumnName("IdTipoInstalacao");
            Property(u => u.TipoMaquinaId).HasColumnName("IdTipoMaquina");
            Property(u => u.ContaContabilId).HasColumnName("IdContaContabil");
            Property(u => u.EquipeId).HasColumnName("IdEquipe");
            Property(u => u.BancoId).HasColumnName("IdBanco");
            Property(u => u.PCId).HasColumnName("IdPc");
            Property(u => u.NrOrdemServico).HasColumnName("NrOrdemServico");


        //HasRequired(u => u.TipoFixacao).WithMany().Map(m => m.MapKey("IdTipoFixacao"));
        //HasRequired(u => u.TipoInstalacao).WithMany().Map(m => m.MapKey("IdTipoInstalacao"));
        //HasRequired(u => u.TipoMaquina).WithMany().Map(m => m.MapKey("IdTipoMaquina"));
        //HasRequired(u => u.ContaContabil).WithMany().Map(m => m.MapKey("IdContaContabil"));
        //HasRequired(u => u.Equipe).WithMany().Map(m => m.MapKey("IdEquipe"));
        //HasRequired(u => u.Banco).WithMany().Map(m => m.MapKey("IdBanco"));
        //HasRequired(u => u.PC).WithMany().Map(m => m.MapKey("IdPc"));

    }
    }
}
