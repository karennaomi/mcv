using MCV.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCV.Infra.Mappings
{
    public class SocioMap : EntityTypeConfiguration<Socio>
    {
            public SocioMap()
            {
                ToTable("TB_Empresa_Socio");
                HasKey(x => x.IdSocio);
                Property(x => x.IdSocio).HasColumnName("IdSocio");
                Property(x => x.IdEmpresa).HasColumnName("IdEmpresa");
                Property(x => x.NomeSocio).HasColumnName("NomeSocio");
                Property(x => x.DtInclusao).HasColumnName("DtInclusao");
            }
        }
}
