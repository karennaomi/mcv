using RestSharp;

namespace LM.Core.Application
{
    public class RestServiceWithRestSharp : IRestService
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
            var taskResponse = _pushServiceClient.ExecutePostTaskAsync(request);
        }
    }
}
