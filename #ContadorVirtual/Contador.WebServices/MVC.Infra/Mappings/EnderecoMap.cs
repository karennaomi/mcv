using MCV.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCV.Infra.Mappings
{
    public class EnderecoMap:EntityTypeConfiguration<Endereco>
    {
        public EnderecoMap()
        {
            ToTable("TB_Endereco");
            HasKey(x => x.Id);
            Property(x => x.Logradouro);
            Property(x => x.Cidade);
            Property(x => x.UF);
            Property(x => x.Numero);
            Property(x => x.Complemento);
            Property(x => x.Cep);
        }

    }
}
