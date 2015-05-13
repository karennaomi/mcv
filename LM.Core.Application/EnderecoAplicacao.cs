using LM.Core.Domain;
using LM.Core.Domain.Servicos;
using System;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface ICepAplicacao
    {
        Endereco BuscarPorCep(string cep);
    }

    public interface IEnderecoAplicacao : ICepAplicacao
    {
        IList<Endereco> Listar(string logradouro, string numero);
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
            if (_servicoRest.Host == null) _servicoRest.Host = new Uri("http://maps.googleapis.com/maps/api/geocode/");
        }

        public Endereco BuscarPorCep(string cep)
        {
            foreach (var servicoDeCep in _servicosDeCep)
            {
                try
                {
                    var endereco = servicoDeCep.BuscarPorCep(cep);
                    return endereco;

                }
                catch
                {
                    continue;
                }
            }
            throw new ApplicationException("Não foi possível localizar um endereço por cep.");
        }

        public IList<Endereco> Listar(string logradouro, string numero)
        {
            if (string.IsNullOrWhiteSpace(logradouro) || string.IsNullOrWhiteSpace(numero)) return null;
            var enderecoGoogle = _servicoRest.Get<GoogleMapsGeocode>(string.Format("/json?address={0}+{1}&sensor=false", logradouro, numero));
            return enderecoGoogle.ListarEnderecos();
        }

        public Endereco BuscarPorPonto(decimal lat, decimal lng)
        {
            var enderecoGoogle = _servicoRest.Get<GoogleMapsGeocode>(string.Format("/json?latlng={0},{1}&sensor=false", lat, lng));
            return enderecoGoogle.ObterEndereco();
        }
    }
}
