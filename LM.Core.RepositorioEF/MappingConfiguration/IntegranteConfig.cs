using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class IntegranteConfig : EntityTypeConfiguration<Integrante>
    {
        public IntegranteConfig()
        {
            ToTable("TB_INTEGRANTE");
            HasKey(i => i.Id);
            Property(i => i.Id).HasColumnName("ID_INTEGRANTE");
            Property(i => i.Nome).HasColumnName("NM_INTEGRANTE");
            Property(i => i.DataNascimento).HasColumnName("DT_NASCIMENTO").IsOptional();
            Property(i => i.EhUsuarioConvidado).HasColumnName("FL_USUARIO_CONVIDADO");
            Property(i => i.DataConvite).HasColumnName("DT_CONVITE").IsOptional();
            Property(i => i.DataInclusao).HasColumnName("DT_INC").IsOptional();
            Property(i => i.DataAlteracao).HasColumnName("DT_ALT").IsOptional();
            Property(i => i.Ativo).HasColumnName("FL_ATIVO");
            Property(i => i.Telefone).HasColumnName("TX_NUMERO_TELEFONE");
            Property(i => i.Sexo).HasColumnName("TX_SEXO");
            Property(i => i.Email).HasColumnName("TX_EMAIL");
            Property(i => i.Cpf).HasColumnName("NR_CPF");
            Property(i => i.Tipo).HasColumnName("ID_TIPO");

            HasRequired(i => i.GrupoDeIntegrantes).WithMany().Map(m => m.MapKey("ID_GRUPO_INTEGRANTE"));
        }
    }
}
