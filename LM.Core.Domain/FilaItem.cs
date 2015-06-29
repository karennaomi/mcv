using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace LM.Core.Domain
{
    public enum TipoFilaItem
    {
        Mensagem = 1,
        Produto = 2
    }

    public abstract class FilaItem
    {
        protected FilaItem()
        {
            DataInclusao = DateTime.Now;
            TipoServicoId = 1; //EXEC
            Origem = "LM.Core";
        }

        public long Id { get; set; }
        public int TipoServicoId { get; set; }
        public string Origem { get; set; }
        public int? OrdemExecucao { get; set; }
        public string StatusFila { get; set; }
        public string StatusProcessamento { get; set; }
        public DateTime? DataInclusao { get; set; }
        public DateTime? DataAlteracao { get; set; }
    }

    public class FilaItemMensagem : FilaItem
    {
        public FilaItemMensagem()
        {
            OrdemExecucao = 1;
            StatusFila = "A";
        }

        public virtual ICollection<FilaMensagem> FilaMensagens { get; set; }

        public void AdicionarMensagem(FilaMensagem filaMensagem)
        {
            if(FilaMensagens == null) FilaMensagens = new Collection<FilaMensagem>();
            FilaMensagens.Add(filaMensagem);
        }
    }

    public class FilaItemProduto : FilaItem
    {
        public virtual ICollection<FilaProduto> FilaProdutos { get; set; }

        public void AdicionarMensagem(FilaProduto filaProduto)
        {
            if (FilaProdutos == null) FilaProdutos = new Collection<FilaProduto>();
            FilaProdutos.Add(filaProduto);
        }
    }
}
