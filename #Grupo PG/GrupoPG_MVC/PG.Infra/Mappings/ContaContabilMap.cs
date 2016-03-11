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
    public class ContaContabilMap: EntityTypeConfiguration<ContaContabil>
    {
        public ContaContabilMap()
        {
            ToTable("TB_CONTA_CONTABIL");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("IdContaContabil");
            Property(x => x.NomeConta).HasColumnName("NomeConta");

        }
    }
}
