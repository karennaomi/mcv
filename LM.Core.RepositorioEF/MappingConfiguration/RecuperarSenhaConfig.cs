using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class RecuperarSenhaConfig : EntityTypeConfiguration<RecuperarSenha>
    {
        public RecuperarSenhaConfig()
        {
            ToTable("TB_USUARIO_RECUPERAR_SENHA");
            HasKey(g => g.Id);
            Property(g => g.Id).HasColumnName("ID_RECUPERAR_SENHA");
            Property(g => g.Token).HasColumnName("TX_TOKEN");
            Property(g => g.DataInclusao).HasColumnName("DT_INC");

            HasRequired(g => g.Usuario).WithMany().Map(m => m.MapKey("ID_USUARIO"));
        }
    }
}
