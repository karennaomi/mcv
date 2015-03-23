﻿namespace LM.Core.RepositorioEF.MappingConfiguration
{
    public class EnderecoConfig : EntityTypeConfiguration<Endereco>
    {
        public EnderecoConfig()
        {
            ToTable("TB_ENDERECO");
            HasKey(e => e.Id);
            Property(e => e.Id).HasColumnName("ID_ENDERECO");
            Property(e => e.Descricao).HasColumnName("NM_ENDERECO");
            Property(e => e.Numero).HasColumnName("NR_ENDERECO");
            Property(e => e.Complemento).HasColumnName("NM_ENDERECO_COMPLEMENTO");
            Property(e => e.Alias).HasColumnName("TX_ENDERECO_ALIAS");
            Property(e => e.Cep).HasColumnName("NR_ENDERECO_CEP");
            Property(e => e.Bairro).HasColumnName("NM_ENDERECO_BAIRRO");
            Property(e => e.Latitude).HasColumnName("NR_ENDERECO_LATITUDE");
            Property(e => e.Longitude).HasColumnName("NR_ENDERECO_LONGITUDE");
            Property(e => e.DataInclusao).HasColumnName("DT_INC").IsOptional();
            Property(e => e.DataAlteracao).HasColumnName("DT_ALT").IsOptional();

            HasRequired(e => e.Cidade).WithMany().Map(m => m.MapKey("ID_CIDADE"));
        }
    }

    public class CidadeConfig : EntityTypeConfiguration<Cidade>
    {
        public CidadeConfig()
        {
            ToTable("TB_CIDADE");
            HasKey(c => c.Id);
            Property(e => e.Id).HasColumnName("ID_CIDADE");
            Property(e => e.Nome).HasColumnName("NM_CIDADE");
            HasRequired(e => e.Uf).WithMany().Map(m => m.MapKey("ID_UF"));
        }
    }

    public class UfConfig : EntityTypeConfiguration<Uf>
    {
        public UfConfig()
        {
            ToTable("TB_UF");
            HasKey(c => c.Id);
            Property(e => e.Id).HasColumnName("ID_UF");
            Property(e => e.Nome).HasColumnName("NM_UF");
            Property(e => e.Sigla).HasColumnName("SG_UF");
        }
    }
}