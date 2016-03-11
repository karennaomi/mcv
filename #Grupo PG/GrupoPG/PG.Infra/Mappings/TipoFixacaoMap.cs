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
    public class TipoFixacaoMap : EntityTypeConfiguration<TipoFixacao>
    {
        public TipoFixacaoMap()
        {
            ToTable("TB_TIPO_FIXACAO");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("IdTipoFixacao");
            Property(x => x.NomeTipoFixacao).HasColumnName("NomeTipoFixacao");

        }
    }
}
