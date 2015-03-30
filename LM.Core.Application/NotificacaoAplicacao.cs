using RestSharp;

namespace LM.Core.Application
{
    public interface INotificacaoAplicacao
    {
        void EnviarNotificacao(string deviceType, string deviceId, string message, string action);
    }

    public class NotificacaoAplicacao : INotificacaoAplicacao
    {
        private readonly RestClient _pushServiceClient;

        public NotificacaoAplicacao(string pushServiceHost)
        {
            _pushServiceClient = new RestClient(pushServiceHost);
        }

        public void EnviarNotificacao(string deviceType, string deviceId, string message, string action)
        {
            var request = new RestRequest("sendpushmessage", Method.POST);
            request.AddHeader("Content-Type", "application/json; charset=utf-8");
            request.AddJsonBody(new
            {
                DeviceId = deviceId,
                DeviceType = deviceType,
                Message = message,
                Action = action

            });
            
            var response = _pushServiceClient.Execute(request);
            var a = response.Content;

        }
    }
}
