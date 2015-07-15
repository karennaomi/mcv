using System;

namespace LM.Core.Domain
{
    public class ProdutoPreco
    {
        public ProdutoPreco()
        {
            Ativo = true;
            DataInclusao = DateTime.Now;
        }

        public int Id { get; set; }
        public decimal? PrecoMin { get; set; }
        public decimal? PrecoMax { get; set; }
        public DateTime? DataPreco { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
