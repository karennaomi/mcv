using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.Domain
{
    public class GoogleMapsGeocode
    {
        [JsonProperty("results")]
        public List<GoogleResult> Results { get; set; }
        
        [JsonProperty("status")]
        public string Status { get; set; }

        public List<Endereco> ListarEnderecos()
        {
            var lista = new List<Endereco>();
            Results.ToList().ForEach(r => lista.Add(r.CreateEndereco()));
            return lista;
        }
    }
}
