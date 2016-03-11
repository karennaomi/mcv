using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Domain;

namespace WebApi.Infra.Mappings
{
    public class ContatosMap: EntityTypeConfiguration<Contatos>
    {
        ContatosMap()
        {
            ToTable("TB_CONTATOS");
            HasKey(x => x.Id);
            Property(x => x.Nome);
            Property(x => x.telefone);
            Property(x => x.Data);
            HasRequired(x => x.Operadora);
        }
    }
}
