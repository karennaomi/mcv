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
    public class EnderecoMap : EntityTypeConfiguration<Endereco>
    {
        public EnderecoMap()
        {
            ToTable("TB_ENDERECO");
            HasKey(x => x.Id);
            Property(x => x.Logradouro).IsRequired();
            Property(x => x.Numero).IsOptional();
            Property(x => x.Complemento).IsOptional();
            Property(x => x.Cidade);
            Property(x => x.UF);
        }
    }
}
