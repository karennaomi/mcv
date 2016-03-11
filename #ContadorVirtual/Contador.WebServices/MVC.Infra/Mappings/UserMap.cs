using MCV.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCV.Infra.Mappings
{
    public class UserMap: EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            ToTable("TB_Usuario");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("IdUsuario");
            Property(x => x.Nome).HasColumnName("NmUsuario");
            Property(x => x.Email).HasColumnName("Email");
            Property(x => x.CPF).HasColumnName("CPF");
            Property(x => x.DDD).HasColumnName("DDD").IsOptional();
            Property(x => x.Telefone).HasColumnName("Telefone").IsOptional();
            Property(x => x.DataInclusao).HasColumnName("DtInclusao");
           

        }
    }
}
