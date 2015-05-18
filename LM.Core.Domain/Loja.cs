using System;

namespace LM.Core.Domain
{
    public class Loja
    {
        public Loja()
        {
            DataInclusao = DataAlteracao = DateTime.Now;
        }

        public int Id { get; set; }
        public string Nome { get; set; }
        public string LocalizadorId { get; set; }
        public string LocalizadorOrigem { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public virtual LojaInfo Info { get; set; }
    }

    public class LojaInfo
    {
        public LojaInfo()
        {
            DataInclusao = DataAlteracao = DateTime.Now;
        }

        public int Id { get; set; }
        public string RazaoSocial { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public virtual Endereco Endereco { get; set; }
    }
     
}
