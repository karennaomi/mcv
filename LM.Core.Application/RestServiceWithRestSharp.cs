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

        public void Post(string endPoint, object content)
        {
            var request = new RestRequest(endPoint);
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddJsonBody(content);
            var response = _pushServiceClient.Post(request);
            CheckResponse(response);
        }

        public Task PostAsync(string endPoint, object content)
        {
            var request = new RestRequest(endPoint);
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddJsonBody(content);
            return _pushServiceClient.ExecutePostTaskAsync(request);
        }

        public void Get(string endpoint)
        {
            var request = new RestRequest(endpoint);
            var response = _pushServiceClient.Get(request);
            CheckResponse(response);
        }

        public T Get<T>(string endpoint) where T : new()
        {
            var request = new RestRequest(endpoint);
            var response = _pushServiceClient.Get<T>(request);
            CheckResponse(response);
            return response.Data;
        }

        private static void CheckResponse(IRestResponse response)
        {
            if (response.StatusCode == HttpStatusCode.NotFound) throw new ObjetoNaoEncontradoException("O recurso buscado não foi encontrado");
            if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent && response.StatusCode != HttpStatusCode.Created) throw new ApplicationException("Ocorreu um erro na requisição com o serviço.");
        }

        public Uri Host
        {
            get { return _pushServiceClient.BaseUrl; }
            set { _pushServiceClient.BaseUrl = value; }
        }
    }
}
