using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace LM.Core.Domain
{
    public class GoogleMapsGeocode
    {
        [JsonProperty("results")]
        public IList<GoogleMapsGeocodeResult> Results { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }

        public Endereco ObterEndereco()
        {
            var firstResult = Results.FirstOrDefault();
            if(firstResult == null) throw new ApplicationException("Não foi localizado nenhum endereço.");
            return new Endereco
            {
                Logradouro = GetComponentShortNameValue(firstResult, "route"),
                Numero = GetNumber(firstResult),
                Bairro = GetComponentShortNameValue(firstResult, "neighborhood"),
                Cep = GetComponentShortNameValue(firstResult, "postal_code"),
                Cidade = new Cidade
                {
                    Nome = GetComponentShortNameValue(firstResult, "administrative_area_level_2"),
                    Uf = new Uf { Sigla = GetComponentShortNameValue(firstResult, "administrative_area_level_1"), }
                },
                Latitude = firstResult.Geometry.Location.Lat,
                Longitude = firstResult.Geometry.Location.Lng,
            };

        }

        private static int GetNumber(GoogleMapsGeocodeResult result)
        {
            var componentValue = GetComponentShortNameValue(result, "street_number");
            int addressNumber;
            if(int.TryParse(componentValue, out addressNumber)) return addressNumber;
            throw new ApplicationException("O número da rua é inválido: " + componentValue);
        }

        private static string GetComponentShortNameValue(GoogleMapsGeocodeResult result, string componentName)
        {
            var addressComponent = result.AddressComponents.SingleOrDefault(c => c.Types.Any(t => t == componentName));
            if (addressComponent == null) throw new ApplicationException("Não achou o componente do endereço");
            return addressComponent.ShortName;
        }
    }

    public class GoogleMapsGeocodeResult
    {
        [JsonProperty("address_components")]
        public IList<GoogleMapsGeocodeAddressComponent> AddressComponents { get; set; }
        [JsonProperty("formatted_address")]
        public string FormattedAddress { get; set; }
        [JsonProperty("geometry")]
        public GoogleMapsGeocodeGeometry Geometry { get; set; }
        [JsonProperty("place_id")]
        public string PlaceId { get; set; }
    }

    public class GoogleMapsGeocodeAddressComponent
    {
        [JsonProperty("long_name")]
        public string LongName { get; set; }
        [JsonProperty("short_name")]
        public string ShortName { get; set; }
        [JsonProperty("types")]
        public IList<string> Types { get; set; }
    }

    public class GoogleMapsGeocodeGeometry
    {
        [JsonProperty("location")]
        public GoogleMapsGeocodeLocation Location { get; set; }
    }

    public class GoogleMapsGeocodeLocation
    {
        [JsonProperty("lat")]
        public decimal Lat { get; set; }
        [JsonProperty("lng")]
        public decimal Lng { get; set; }
    }
}
