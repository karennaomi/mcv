using LM.Core.Domain;
using LM.Core.Domain.Servicos;
using System;
using System.Collections.Generic;
using Ninject;

namespace LM.Core.Application
{
    public interface ICepAplicacao
    {
        Endereco BuscarPorCep(string cep);
    }

    public interface IEnderecoAplicacao : ICepAplicacao
    {
        IList<Endereco> BuscarPorEndereco(string endereco);
        IList<Endereco> BuscarPorPonto(string lat, string lng);
    }

    public class EnderecoAplicacao : IEnderecoAplicacao
    {
        private readonly IServicoRest _servicoRest;
        private readonly ICepAplicacao _servicoDeCep;
        public EnderecoAplicacao(ICepAplicacao servicoDeCep, [Named("GoogleGeocodeService")]IServicoRest servicoRest)
        {
            _servicoDeCep = servicoDeCep;
            _servicoRest = servicoRest;
            if (_servicoRest.Host == null) _servicoRest.Host = new Uri("http://maps.googleapis.com/maps/api/geocode/");
        }

        public Endereco BuscarPorCep(string cep)
        {
            return _servicoDeCep.BuscarPorCep(cep);
       }

        public IList<Endereco> BuscarPorEndereco(string endereco)
        {
            var enderecoGoogle = _servicoRest.Get<GoogleMapsGeocode>(string.Format("/json?address={0}&sensor=false", endereco));
            return enderecoGoogle.ListarEnderecos();
        }

        public IList<Endereco> BuscarPorPonto(string lat, string lng)
        {
            var enderecoGoogle = _servicoRest.Get<GoogleMapsGeocode>(string.Format("/json?latlng={0},{1}&sensor=false", lat, lng));
            return enderecoGoogle.ListarEnderecos();
        }
    }
}
