using System;
using System.Net;
using System.Threading.Tasks;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Servicos;
using RestSharp;

namespace LM.Core.Application
{
    public class RestServiceWithRestSharp : IServicoRest
    {
        private readonly RestClient _pushServiceClient;
        public RestServiceWithRestSharp(string host)
        {
            _pushServiceClient = new RestClient(host);
        }
        
        public Task Post(string endPoint, object content)
        {
            var request = new RestRequest(endPoint);
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddJsonBody(content);
            return _pushServiceClient.ExecutePostTaskAsync(request);
        }

        public T Get<T>(string endpoint) where T : new()
        {
            var request = new RestRequest(endpoint);
            var response = _pushServiceClient.Get<T>(request);
            switch (response.ResponseStatus)
            {
                case ResponseStatus.Error:
                case ResponseStatus.Aborted:
                    if (response.StatusCode == HttpStatusCode.NotFound) throw new ObjetoNaoEncontradoException("O recurso buscado não foi encontrado");
                    throw new ApplicationException("Ocorreu um erro na requisição com o serviço.");
                case ResponseStatus.TimedOut:
                    throw new ApplicationException("O tempo de execução do serviço esgotou.");
                default:
                    return response.Data;
            }
        }

        public Uri Host
        {
            get { return _pushServiceClient.BaseUrl; }
            set { _pushServiceClient.BaseUrl = value; }
        }
    }
}
