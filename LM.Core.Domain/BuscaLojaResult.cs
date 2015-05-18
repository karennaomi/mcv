using System.Collections.Generic;

namespace LM.Core.Domain
{
    public class BuscaLojaResult
    {
        public IList<Loja> Lojas { get; set; } 
        public string NextPageToken { get; set; } 
    }
}