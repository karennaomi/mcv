using LM.Core.Domain;
using LM.Core.Domain.Servicos;
using System;
using Ninject;

namespace LM.Core.Application
{
    public interface IPlacesService
    {
        BuscaLojaResult BuscarLojas(decimal lat, decimal lng, string nextPageToken, int radius = 1000);
        Loja BuscarDetalheLoja(string localizadorId);
    }

    public class GooglePlaceService : IPlacesService
    {
        private readonly string _key;
        private readonly IServicoRest _servicoRest;
        public GooglePlaceService([Named("GooglePlaceService")]IServicoRest restService, string key)
        {
            _servicoRest = restService;
            if (_servicoRest.Host == null) _servicoRest.Host = new Uri("https://maps.googleapis.com/maps/api/place/");
            _key = key;
        }

        public BuscaLojaResult BuscarLojas(decimal lat, decimal lng, string nextPageToken = "", int radius = 1000)
        {
            var endPoint = string.Format("/search/json?location={0},{1}&radius={2}&key={3}&sensor=false&rank_by=distance&types=grocery_or_supermarket", lat, lng, radius, _key);
            if (!string.IsNullOrEmpty(nextPageToken)) endPoint += string.Format("&next_page_token={0}", nextPageToken);
            var search = _servicoRest.Get<GooglePlaceSearch>(endPoint);
            return search.ObterLojasResult();
        }

        public Loja BuscarDetalheLoja(string localizadorId)
        {
            var endPoint = string.Format("/details/json?placeid={0}&key={1}&sensor=false", localizadorId, _key);
            var detail = _servicoRest.Get<GooglePlaceDetail>(endPoint);
            return detail.ObterLoja();
        }
    }
}
