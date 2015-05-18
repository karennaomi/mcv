using LM.Core.Domain;
using LM.Core.Domain.Servicos;
using System;

namespace LM.Core.Application
{
    public class EnderecoViaCepService : ICepAplicacao
    {
        private readonly IServicoRest _servicoRest;
        public EnderecoViaCepService(IServicoRest servicoRest)
        {
            _servicoRest = servicoRest;
            if (_servicoRest.Host == null) _servicoRest.Host = new Uri("http://viacep.com.br/ws/");
        }
        public Endereco BuscarPorCep(string cep)
        {
            var enderecoPostmon = _servicoRest.Get<EnderecoViaCep>(string.Format("/{0}/json/", cep));
            return enderecoPostmon.ObterEndereco();
        }
    }
}
