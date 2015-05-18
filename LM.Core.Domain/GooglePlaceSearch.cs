using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.Domain
{
    public class GooglePlaceSearch
    {
        [JsonProperty("next_page_token")]
        public string NextPageToken { get; set; }

        [JsonProperty("results")]
        public List<GooglePlaceSearchResult> Results { get; set; }
        
        [JsonProperty("status")]
        public string Status { get; set; }

        public BuscaLojaResult ObterLojasResult()
        {
            return new BuscaLojaResult
            {
                NextPageToken = NextPageToken,
                Lojas = ListarLojas()
            };
        }

        private IList<Loja> ListarLojas()
        {
            var lista = new List<Loja>();
            Results.ToList().ForEach(r => lista.Add(r.CreateLoja()));
            return lista;
        }
    }
}
