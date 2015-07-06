using System;

namespace LM.Core.Domain
{
    public class Contrato
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public int Numero { get; set; }
        public DateTime InicioVigencia { get; set; }
        public DateTime FimVigencia { get; set; }
        public string Conteudo { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }
}
