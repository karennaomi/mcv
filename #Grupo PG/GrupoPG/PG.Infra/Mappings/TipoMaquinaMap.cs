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
    public class TipoMaquinaMap : EntityTypeConfiguration<TipoMaquina>
    {
        public TipoMaquinaMap()
        {
            ToTable("TB_TIPO_MAQUINA");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("IdTipoMaquina");
            Property(x => x.NomeTipoMaquina).HasColumnName("NomeTipoMaquina");

        }
    }
}
