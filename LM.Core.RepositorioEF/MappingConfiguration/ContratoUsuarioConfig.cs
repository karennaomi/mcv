
using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class ContratoUsuarioConfig : EntityTypeConfiguration<ContratoUsuario>
    {
        public ContratoUsuarioConfig()
        {
            ToTable("TB_Contrato_Usuario");
            HasKey(u => u.Id);
            Property(u => u.Id).HasColumnName("ID_CONTRATO_USUARIO");
            Property(u => u.ContratoId).HasColumnName("ID_CONTRATO");
            Property(u => u.PontoDemandaId).HasColumnName("ID_PONTO_DEMANDA");
            Property(i => i.DataInclusao).HasColumnName("DT_INC").IsOptional();
            Property(i => i.DataAlteracao).HasColumnName("DT_ALT").IsOptional();
        }
    }
}
