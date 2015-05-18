using Newtonsoft.Json;

namespace LM.Core.Domain
{
    public class GooglePlaceSearchResult : GoogleResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }

        [JsonProperty("vicinity")]
        public string Vicinity { get; set; }

        [JsonProperty("website")]
        public string Website { get; set; }

        public Loja CreateLoja()
        {
            return new Loja
            {
                LocalizadorId = PlaceId,
                LocalizadorOrigem = "google",
                Nome = Name,
                Info = new LojaInfo
                {
                    Endereco = CreateEndereco()
                }
            };
        }
    }
}