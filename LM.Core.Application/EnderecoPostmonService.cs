using LM.Core.Domain;
using LM.Core.Domain.Servicos;
using System;

namespace LM.Core.Application
{
    public class EnderecoPostmonService : ICepAplicacao
    {
        private readonly IServicoRest _servicoRest;
        public EnderecoPostmonService(IServicoRest servicoRest)
        {
            _servicoRest = servicoRest;
        }
        public Endereco BuscarPorCep(string cep)
        {
            if (_servicoRest.Host == null) _servicoRest.Host = new Uri("http://api.postmon.com.br/v1/");
            var enderecoPostmon = _servicoRest.Get<EnderecoPostmon>(string.Format("/cep/{0}", cep));
            return enderecoPostmon.ObterEndereco();
        }
    }
}
