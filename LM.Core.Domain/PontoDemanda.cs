using System;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.Domain
{
    public class PontoDemanda
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public short? QuantidadeDiasAlertaReposicao { get; set; }
        public short? QuantidadeDiasCoberturaEstoque { get; set; }
        public decimal? FatorZNivelServico { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
        public TipoPontoDemanda? Tipo { get; set; }

        public virtual Endereco Endereco { get; set; }
        public virtual GrupoDeIntegrantes GrupoDeIntegrantes { get; set; }
        public virtual ICollection<Lista> Listas { get; set; }

        public Usuario Usuario {
            get { return GrupoDeIntegrantes.Integrantes.Single(i => i.EhUsuarioSistema).Usuario; }
        }
    }
}
