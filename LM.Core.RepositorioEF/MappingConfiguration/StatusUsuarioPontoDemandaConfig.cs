namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class StatusUsuarioPontoDemandaConfig : EntityTypeConfiguration<StatusUsuarioPontoDemanda>
    {
        public StatusUsuarioPontoDemandaConfig()
        {
            ToTable("TB_STATUS_USUARIO_PONTO_DEMANDA");
            HasKey(u => u.Id);
            Property(u => u.Id).HasColumnName("ID_STATUS_USUARIO_PONTO_DEMANDA");
            Property(u => u.DataInclusao).HasColumnName("DT_INC").IsOptional();
            Property(u => u.DataAlteracao).HasColumnName("DT_ALT").IsOptional();
            Property(u => u.PontoDemandaId).HasColumnName("ID_PONTO_REAL_DEMANDA").IsOptional();
            Property(u => u.StatusCadastro).HasColumnName("ID_STATUS_CADASTRO");
        }
    }
}
