using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LM.Core.Domain
{
    public class GoogleResult
    {
        [JsonProperty("address_components")]
        public List<GoogleAddressComponent> AddressComponents { get; set; }
        
        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }
        
        [JsonProperty("geometry")]
        public GoogleGeometry Geometry { get; set; }
        
        [JsonProperty("place_id")]
        public string PlaceId { get; set; }

        public Endereco CreateEndereco()
        {
            return new Endereco
            {
                Logradouro = GetComponentShortNameValue("route"),
                Numero = GetNumber(),
                Bairro = GetComponentShortNameValue("neighborhood"),
                Cep = GetComponentShortNameValue("postal_code"),
                Cidade = new Cidade
                {
                    Nome = GetComponentShortNameValue("administrative_area_level_2"),
                    Uf = new Uf { Sigla = GetComponentShortNameValue("administrative_area_level_1"), }
                },
                Latitude = Geometry.Location.Lat,
                Longitude = Geometry.Location.Lng,
            };
        }

        private int GetNumber()
        {
            var componentValue = GetComponentShortNameValue("street_number");
            int addressNumber;
            return int.TryParse(componentValue, out addressNumber) ? addressNumber : 0;
        }

        private string GetComponentShortNameValue(string componentName)
        {
            var addressComponent = AddressComponents.SingleOrDefault(c => c.Types.Any(t => t == componentName));
            return addressComponent == null ? "" : addressComponent.ShortName;
        }
    }

    public class GoogleGeometry
    {
        [JsonProperty("location")]
        public GoogleLocation Location { get; set; }
    }

    public class GoogleLocation
    {
        [JsonProperty("lat")]
        public decimal Lat { get; set; }
        [JsonProperty("lng")]
        public decimal Lng { get; set; }
    }

    public class GoogleAddressComponent
    {
        [JsonProperty("long_name")]
        public string LongName { get; set; }

        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        [JsonProperty("types")]
        public List<string> Types { get; set; }
    }
}