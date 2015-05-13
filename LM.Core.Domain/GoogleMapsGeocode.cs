using System;
using System.Collections.Generic;
using System.Linq;
using LM.Core.Domain.CustomException;
using Newtonsoft.Json;

namespace LM.Core.Domain
{
    public class GoogleMapsGeocode
    {
        [JsonProperty("results")]
        public List<GoogleMapsGeocodeResult> Results { get; set; }
        [JsonProperty("status")]
        public string Status { get; set; }

        public Endereco ObterEndereco()
        {
            var firstResult = Results.FirstOrDefault();
            if(firstResult == null) throw new ObjetoNaoEncontradoException("Não foi localizado nenhum endereço.");
            return CreateEndereco(firstResult);
        }

        public List<Endereco> ListarEnderecos()
        {
            var lista = new List<Endereco>();
            Results.ToList().ForEach(r => lista.Add(CreateEndereco(r)));
            return lista;
        }

        private static Endereco CreateEndereco(GoogleMapsGeocodeResult result)
        {
            return new Endereco
            {
                Logradouro = GetComponentShortNameValue(result, "route"),
                Numero = GetNumber(result),
                Bairro = GetComponentShortNameValue(result, "neighborhood"),
                Cep = GetComponentShortNameValue(result, "postal_code"),
                Cidade = new Cidade
                {
                    Nome = GetComponentShortNameValue(result, "administrative_area_level_2"),
                    Uf = new Uf { Sigla = GetComponentShortNameValue(result, "administrative_area_level_1"), }
                },
                Latitude = result.Geometry.Location.Lat,
                Longitude = result.Geometry.Location.Lng,
            };
        }

        private static int GetNumber(GoogleMapsGeocodeResult result)
        {
            var componentValue = GetComponentShortNameValue(result, "street_number");
            int addressNumber;
            return int.TryParse(componentValue, out addressNumber) ? addressNumber : 0;
        }

        private static string GetComponentShortNameValue(GoogleMapsGeocodeResult result, string componentName)
        {
            var addressComponent = result.AddressComponents.SingleOrDefault(c => c.Types.Any(t => t == componentName));
            return addressComponent == null ? "" : addressComponent.ShortName;
        }
    }

    public class GoogleMapsGeocodeResult
    {
        [JsonProperty("address_components")]
        public List<GoogleMapsGeocodeAddressComponent> AddressComponents { get; set; }
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
        public List<string> Types { get; set; }
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
