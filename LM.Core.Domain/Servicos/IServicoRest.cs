using System;
using System.Threading.Tasks;

namespace LM.Core.Domain.Servicos
{
    public interface IServicoRest
    {
        Uri Host { get; set; }
        Task Post(string endPoint, object content);
        T Get<T>(string endPoint) where T : new();
    }
}
