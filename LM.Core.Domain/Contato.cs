using System;

namespace LM.Core.Domain
{
    public class Contato
    {
        public Contato()
        {
            DataInclusao = DateTime.Now;
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Mensagem { get; set; }
        public DateTime DataInclusao { get; set; }  
    }
}
