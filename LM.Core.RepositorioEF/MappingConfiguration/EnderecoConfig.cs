using LM.Core.Domain;
using System.Data.Entity.ModelConfiguration;

namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class EnderecoConfig : EntityTypeConfiguration<Endereco>
    {
        public EnderecoConfig()
        {
            ToTable("TB_ENDERECO");
            HasKey(e => e.Id);
            Property(e => e.Id).HasColumnName("ID_ENDERECO");
            Property(e => e.Logradouro).HasColumnName("NM_ENDERECO");
            Property(e => e.Numero).HasColumnName("NR_ENDERECO").IsOptional();
            Property(e => e.Complemento).HasColumnName("NM_ENDERECO_COMPLEMENTO");
            Property(e => e.Alias).HasColumnName("TX_ENDERECO_ALIAS");
            Property(e => e.Cep).HasColumnName("NR_ENDERECO_CEP");
            Property(e => e.Bairro).HasColumnName("NM_ENDERECO_BAIRRO");
            Property(e => e.Latitude).HasColumnName("NR_ENDERECO_LATITUDE").HasColumnType("decimal").HasPrecision(20, 15);
            Property(e => e.Longitude).HasColumnName("NR_ENDERECO_LONGITUDE").HasColumnType("decimal").HasPrecision(20, 15);
            Property(e => e.DataInclusao).HasColumnName("DT_INC").IsOptional();
            Property(e => e.DataAlteracao).HasColumnName("DT_ALT").IsOptional();
            Ignore(e => e.Cidade);
            Ignore(e => e.Uf);
        }
    }
}
