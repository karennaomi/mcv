using System;

namespace LM.Core.Domain
{
    public class Plano
    {
        public Plano()
        {
            DataInclusao = DataAlteracao = DateTime.Now;
            Ativo = true;
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public int Periodo { get; set; }
        public decimal Valor { get; set; }
        public string Chamada { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime DataAlteracao { get; set; }
        public bool Ativo { get; set; }

        public decimal ValorTotal()
        {
            return Periodo * Valor;
        }
    }
}
