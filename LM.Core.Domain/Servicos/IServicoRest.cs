
namespace LM.Core.Domain.Servicos
{
    public interface IServicoRest
    {
        void Post(string endPoint, object content);
    }
}
