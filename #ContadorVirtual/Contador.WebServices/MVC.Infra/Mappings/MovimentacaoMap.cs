using MCV.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCV.Infra.Mappings
{
    public class MovimentacaoMap : EntityTypeConfiguration<Movimentacao>
    {
        public MovimentacaoMap()
        {
            ToTable("TB_MOVIMENTACAO");
            HasKey(x => x.idMovimentacao);
            Property(x => x.idMovimentacao).HasColumnName("IdMovimentacao");
            Property(x => x.IdEmpresa).HasColumnName("IdEmpresa");
            Property(x => x.IdTipoMovimentaco).HasColumnName("IdTipoMovimentacao");
            Property(x => x.DtMovimentacao).HasColumnName("DtMovimentacao");
            Property(x => x.VlMovimentacao).HasColumnName("vlMovimentacao").IsOptional();
            Property(x => x.DtInclusao).HasColumnName("DtInclusao").IsOptional();
            


        }
    }
}
