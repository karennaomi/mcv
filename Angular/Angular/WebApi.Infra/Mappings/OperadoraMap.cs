using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Domain;

namespace WebApi.Infra.Mappings
{
    public class OperadoraMap: EntityTypeConfiguration<Operadora>
    {
        OperadoraMap()
        {
            ToTable("tb_operadora");
            Property(x => x.Id);
            Property(x => x.Nome);
            Property(x => x.Codigo);
            Property(x => x.Categoria);
        }
    }
}
