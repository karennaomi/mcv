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
    public class PCMap : EntityTypeConfiguration<PC>
    {
        public PCMap()
        {
            ToTable("TB_PC");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("IdPc");
            Property(x => x.ModeloEquipamento).HasColumnName("NumeroPC");

        }
    }
}
