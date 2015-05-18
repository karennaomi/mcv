using Newtonsoft.Json;

namespace LM.Core.Domain
{
    public class GooglePlaceDetail
    {
        [JsonProperty("result")]
        public GooglePlaceSearchResult Result { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        public Loja ObterLoja()
        {
            return Result.CreateLoja();
        }
    }
}