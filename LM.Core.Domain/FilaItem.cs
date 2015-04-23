using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LM.Core.Domain
{
    public class FilaItem
    {
        public FilaItem()
        {
            DataInclusao = DataAlteracao = DateTime.Now;
            TipoOperacaoId = 1; //MENSAGEM
            TipoServicoId = 1; //EXEC
            Origem = "LM.Core";
            OrdemExecucao = 1;
            StatusFila = "A";
        }

        public long Id { get; set; }
        public int TipoOperacaoId { get; set; }
        public int TipoServicoId { get; set; }
        public string Origem { get; set; }
        public int OrdemExecucao { get; set; }
        public string StatusFila { get; set; }
        public string StatusProcessamento { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }

        public virtual ICollection<FilaMensagem> Mensagens { get; set; }

        public void AdicionarMensagem(FilaMensagem filaMensagem)
        {
            if(Mensagens == null) Mensagens = new Collection<FilaMensagem>();
            Mensagens.Add(filaMensagem);
        }
    }
}
