using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UP.Domain;
namespace UP.Infra.Mappings
{
    public class UserMap: EntityTypeConfiguration<User>
    {
        UserMap()
        {
            ToTable("TB_USUARIO");
            HasKey(x => x.Id);

            Property(i => i.Id).HasColumnName("IdUsuario");
            Property(i => i.Name).HasColumnName("NmUsuario");
            Property(i => i.TipoUsuarioId).HasColumnName("IdTipoUsuario").IsOptional();
            Property(i => i.Email).HasColumnName("Email");
            Property(i => i.Senha).HasColumnName("Senha");

        }
    }
}
