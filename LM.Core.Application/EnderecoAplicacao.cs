using System;
using System.Collections.Generic;
using System.Linq;
using LM.Core.Domain;
using LM.Core.Domain.Servicos;

namespace LM.Core.Application
{
    public interface ICepAplicacao
    {
        Endereco BuscarPorCep(string cep);
    }

    public interface IEnderecoAplicacao : ICepAplicacao
    {
        void PreencherLatitudeLongitude(Endereco endereco);
        Endereco BuscarPorPonto(decimal lat, decimal lng);
    }

    public class EnderecoAplicacao : IEnderecoAplicacao
    {
        private readonly IServicoRest _servicoRest;
        private readonly IList<ICepAplicacao> _servicosDeCep;
        public EnderecoAplicacao(IList<ICepAplicacao> servicosDeCep, IServicoRest servicoRest)
        {
            _servicosDeCep = servicosDeCep;
            _servicoRest = servicoRest;
        }

        public Endereco BuscarPorCep(string cep)
        {
            foreach (var servicoDeCep in _servicosDeCep)
            {
                try
                {
                    var endereco = servicoDeCep.BuscarPorCep(cep);
                    PreencherLatitudeLongitude(endereco);
                    return endereco;

                }
                catch
                {
                    continue;
                }
            }
            throw new ApplicationException("Não foi possível localizar um endereço por cep.");
        }

        public void PreencherLatitudeLongitude(Endereco endereco)
        {
            if (string.IsNullOrWhiteSpace(endereco.Logradouro) || !endereco.Numero.HasValue) return;
            _servicoRest.Host = new Uri("http://maps.googleapis.com/maps/api/geocode/");
            var enderecoGoogle = _servicoRest.Get<GoogleMapsGeocode>(string.Format("/json?address={0}+{1}&sensor=false", endereco.Logradouro, endereco.Numero));
            endereco.Latitude = enderecoGoogle.Results.First().Geometry.Location.Lat;
            endereco.Longitude = enderecoGoogle.Results.First().Geometry.Location.Lng;
        }

        public Endereco BuscarPorPonto(decimal lat, decimal lng)
        {
            _servicoRest.Host = new Uri("http://maps.googleapis.com/maps/api/geocode/");
            var enderecoGoogle = _servicoRest.Get<GoogleMapsGeocode>(string.Format("/json?latlng={0},{1}&sensor=false", lat, lng));
            return enderecoGoogle.ObterEndereco();
        }
    }
}
