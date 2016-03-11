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
    public class BancoMap : EntityTypeConfiguration<Banco>
    {
        public BancoMap()
        {
            ToTable("TB_Banco");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("IdBanco");
            Property(x => x.NomeBanco).HasColumnName("NomeBanco");


        }
    }
}
