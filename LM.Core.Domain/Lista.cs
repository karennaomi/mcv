using System;
using System.Collections.Generic;

namespace LM.Core.Domain
{
    public class Lista
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public string Status { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public virtual PontoDemanda PontoDemanda { get; set; }
        public virtual ICollection<ListaItem> Itens { get; set; }
    }
}
