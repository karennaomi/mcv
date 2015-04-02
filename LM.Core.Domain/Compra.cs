using System;
using System.Collections.Generic;

namespace LM.Core.Domain
{
    public class Compra
    {
        public Compra()
        {
            DataInclusao = DataAlteracao = DateTime.Now;
        }

        public long Id { get; set; }
        public int LojaId { get; set; }

        public DateTime? DataInicioCompra { get; set; }
        public DateTime? DataFimCompra { get; set; }
        public DateTime? DataCapturaPrimeiroItemCompra { get; set; }

        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public virtual PontoDemanda PontoDemanda { get; set; }
        public virtual Integrante Integrante { get; set; }
        public virtual ICollection<CompraItem> Itens { get; set; }

        public void AdicionarItemSubstituto(CompraItem itemOriginal, CompraItem itemSubstituto, string motivo)
        {
            itemSubstituto.Status = StatusCompra.Comprado;
            itemOriginal.Status = StatusCompra.ItemSubstituido;
            itemSubstituto.ItemSubstituto = new CompraItemSubstituto
            {
                Original = itemOriginal,
                Motivo = motivo
            };
            
            Itens.Add(itemOriginal);
            Itens.Add(itemSubstituto);
        }
    }
}
