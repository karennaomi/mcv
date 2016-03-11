using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Domain
{
    public class OrdemServico
    {
        public int Id { get; set; }
        public int BancoId { get; set; }
        public string NrOrdemServico { get; set; }
                                           // public virtual Banco Banco  { get; set; }
        public string Sinalizacao { get; set; }
        public string LocalizacaoMaquina { get; set; }
        public string NomeContato { get; set; }
        public string TelefoneContato { get; set; }
        public bool Habilitada { get; set; }
        public decimal ValorTotal { get; set; }
        public string PaUnificado { get; set; }
        public string Defeito { get; set; }
        // public virtual ContaContabil ContaContabil { get; set; }
        public int ContaContabilId { get; set; }
        public string Serie { get; set; }
        //public virtual TipoMaquina  TipoMaquina { get; set; }
        public int TipoMaquinaId { get; set; }
        //public virtual TipoFixacao TipoFixacao { get; set; }
        public int TipoFixacaoID { get; set; }
        //public TipoInstalacao TipoInstalacao { get; set; }
        public int TipoInstalacaoId { get; set; }
        //public virtual Equipe Equipe { get; set; }
        public int EquipeId { get; set; }
        //public virtual PC PC { get; set; }
        public int PCId { get; set; }
        public DateTime DataEntrega { get; set; }
        public DateTime HoraEntrega { get; set; }
        public DateTime DataFinalizacao { get; set; }
        public DateTime DtChegada { get; set; }
        public DateTime HrChegada { get; set; }
        public DateTime DtChegadaTransportadora { get; set; }
        public DateTime HrChegadaTransportadora { get; set; }




        public OrdemServico()
        {
            //Banco = new Banco();
            //ContaContabil = new ContaContabil();
            //TipoMaquina = new TipoMaquina();
            //TipoFixacao = new TipoFixacao();
            //TipoInstalacao = new TipoInstalacao();
            //PC = new PC();
            //Equipe = new Equipe();
            
        }

    }
}
