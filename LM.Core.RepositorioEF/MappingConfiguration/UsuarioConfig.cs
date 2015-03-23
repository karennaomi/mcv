namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class UsuarioConfig : EntityTypeConfiguration<Usuario>
    {
        public UsuarioConfig()
        {
            ToTable("TB_USUARIO");
            HasKey(u => u.Id);
            Property(u => u.Id).HasColumnName("ID_USUARIO");
            Property(u => u.Nome).HasColumnName("NM_USUARIO");
            Property(u => u.Email).HasColumnName("TX_EMAIL");
            Property(u => u.Login).HasColumnName("TX_LOGIN");
            Property(u => u.Senha).HasColumnName("TX_SENHA");
            Property(u => u.Cpf).HasColumnName("NR_CPF");
            Property(u => u.DataNascimento).HasColumnName("DT_NASCIMENTO");
            Property(u => u.Sexo).HasColumnName("TX_SEXO");
            Property(u => u.Tipo).HasColumnName("ID_TIPO_USUARIO");

            HasRequired(u => u.StatusUsuarioPontoDemanda).WithRequiredPrincipal(s => s.Usuario).Map(m => m.MapKey("ID_USUARIO"));

            Ignore(u => u.Integrante);
            Ignore(u => u.UsuarioIdRedeSocial);
            Ignore(u => u.Origem);
            Ignore(u => u.ImgPerfil);
            
        }
    }
}
