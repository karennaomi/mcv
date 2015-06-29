using System;

namespace LM.Core.Domain
{
    public class FilaProduto
    {
        public FilaProduto()
        {
            DataInclusao = DateTime.Now;
        }

        public long Id { get; set; }
        public string Ean { get; set; }
        public string Descricao { get; set; }
        public string Imagem { get; set; }
        public DateTime? DataInclusao { get; set; }

        public virtual FilaItemProduto FilaItem { get; set; }
    }
}
