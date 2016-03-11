using MCV.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCV.Infra.Mappings
{
    public class EmpresasMap: EntityTypeConfiguration<Empresas>
    {
       public EmpresasMap()
        {
            ToTable("TB_Empresa");
            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("IdEmpresa");
            Property(x => x.NomeFantasia).HasColumnName("NmFantasia");
            Property(x => x.RazaoSocial).HasColumnName("RazaoSocial");
            Property(x => x.OptanteSimples).HasColumnName("OptanteSimples");
            Property(x => x.QtdeFuncionario).HasColumnName("QtdeFuncionario");
            Property(x => x.Ativa).HasColumnName("Ativa");
            Property(x => x.AtivaMCV).HasColumnName("AtivaMCV");
            Property(x => x.CNPJ).HasColumnName("CPNJ");
            //HasRequired(x => x.Endereco);

            
        }
    }
}
