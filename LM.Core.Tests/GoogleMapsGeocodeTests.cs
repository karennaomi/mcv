using System.Linq;
using LM.Core.Domain;
using Newtonsoft.Json;
using NUnit.Framework;

namespace LM.Core.Tests
{
    [TestFixture]
    public class GoogleMapsGeocodeTests
    {
        [Test]
        public void ObterEndereco()
        {
            var result = JsonConvert.DeserializeObject<GoogleMapsGeocode>(Result());
            var endereco = result.ListarEnderecos().First();
            Assert.AreEqual("Av. Interlagos", endereco.Logradouro);
            Assert.AreEqual(100, endereco.Numero);
            Assert.AreEqual("Jardim Umuarama", endereco.Bairro);
            Assert.AreEqual("04660", endereco.Cep);
            Assert.AreEqual("São Paulo", endereco.Cidade.Nome);
            Assert.AreEqual("SP", endereco.Cidade.Uf.Sigla);
            Assert.AreEqual(-23.6542054, endereco.Latitude);
            Assert.AreEqual(-46.6799241, endereco.Longitude);
        }

        private static string Result()
        {
            return
                "{results: [{address_components: [{long_name: \"100\", short_name: \"100\", types: [\"street_number\"] }, {long_name: \"Avenida Interlagos\", short_name: \"Av. Interlagos\", types: [\"route\"] }, {long_name: \"Jardim Umuarama\", short_name: \"Jardim Umuarama\", types: [\"neighborhood\", \"political\"] }, {long_name: \"São Paulo\", short_name: \"São Paulo\", types: [\"locality\", \"political\"] }, {long_name: \"São Paulo\", short_name: \"São Paulo\", types: [\"administrative_area_level_2\", \"political\"] }, {long_name: \"São Paulo\", short_name: \"SP\", types: [\"administrative_area_level_1\", \"political\"] }, {long_name: \"Brazil\", short_name: \"BR\", types: [\"country\", \"political\"] }, {long_name: \"04660\", short_name: \"04660\", types: [\"postal_code_prefix\", \"postal_code\"] } ], formatted_address: \"Avenida Interlagos, 100 - Jardim Umuarama, São Paulo - SP, Brazil\", geometry: {bounds: {northeast: {lat: -23.6542041, lng: -46.6799057 }, southwest: {lat: -23.6542054, lng: -46.6799241 } }, location: {lat: -23.6542054, lng: -46.6799241 }, location_type: \"RANGE_INTERPOLATED\", viewport: {northeast: {lat: -23.65285576970849, lng: -46.6785659197085 }, southwest: {lat: -23.6555537302915, lng: -46.6812638802915 } } }, place_id: \"EkJBdmVuaWRhIEludGVybGFnb3MsIDEwMCAtIEphcmRpbSBVbXVhcmFtYSwgU8OjbyBQYXVsbyAtIFNQLCBCcmFzaWw\", types: [\"street_address\"] }], status: \"OK\"}";
        }
    }
}
