using Contador.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contador.Infra.Mappings
{
   public class UserMap: EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            ToTable("TB_USUARIO");
            HasKey(x => x.Id);

            Property(i => i.Id).HasColumnName("IdUsuario");
            Property(i => i.Nome).HasColumnName("NmUsuario").IsOptional();
            Property(i => i.CPF).HasColumnName("CPF").IsOptional();

        }
    }

}
