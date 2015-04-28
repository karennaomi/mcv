using System;
using System.Collections.Generic;
using System.Linq;

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

        public void Validar()
        {
            if (Itens == null || !Itens.Any()) throw new ApplicationException("A compra deve possuir itens.");
            if (PontoDemanda == null || PontoDemanda.Id == 0) throw new ApplicationException("A compra deve possuir ponto de demanda.");
            if (Integrante == null || Integrante.Id == 0) throw new ApplicationException("A compra deve possuir integrante.");
            if (Integrante.Usuario == null || Integrante.Usuario.Id == 0) throw new ApplicationException("O integrante da compra deve possuir um usuário.");
            if (Itens.Where(i => i.ProdutoId != null).GroupBy(i => i.ProdutoId).Any(g => g.Count() > 1))
                throw new ApplicationException("Existem itens duplicados na sua compra.");
        }
    }
}
