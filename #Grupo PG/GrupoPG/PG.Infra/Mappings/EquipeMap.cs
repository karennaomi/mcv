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
    public class EquipeMap : EntityTypeConfiguration<Equipe>
    {
        public EquipeMap()
        {
            ToTable("TB_EQUIPE");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("IdEquipe");
            Property(x => x.NomeEquipe).HasColumnName("NomeEquipe");

        }
    }
}
