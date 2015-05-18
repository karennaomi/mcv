using Newtonsoft.Json;

namespace LM.Core.Domain
{
    public class GooglePlaceSearchResult : GoogleResult
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("formatted_phone_number")]
        public string FormattedPhoneNumber { get; set; }

        public Loja CreateLoja()
        {
            return new Loja
            {
                LocalizadorId = PlaceId,
                LocalizadorOrigem = "google",
                Nome = Name,
                Telefone = FormattedPhoneNumber,
                Info = new LojaInfo
                {
                    Endereco = CreateEndereco()
                }
            };
        }
    }
}