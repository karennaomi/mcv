using System;
using System.Threading.Tasks;

namespace LM.Core.Domain.Servicos
{
    public interface IServicoRest
    {
        Uri Host { get; set; }
        void Post(string endPoint, object content);
        Task PostAsync(string endPoint, object content);
        void Get(string endPoint);
        T Get<T>(string endPoint) where T : new();
    }
}
