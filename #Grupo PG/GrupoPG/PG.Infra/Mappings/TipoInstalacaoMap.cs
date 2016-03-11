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
    public class TipoInstalacaoMap: EntityTypeConfiguration<TipoInstalacao>
    {
        public TipoInstalacaoMap()
        {
            ToTable("TB_TIPO_INSTALACAO");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("IdTipoInstalacao");
            Property(x => x.NomeTipoInstalacao).HasColumnName("NomeTipoInstalacao");

        }
    }
}
