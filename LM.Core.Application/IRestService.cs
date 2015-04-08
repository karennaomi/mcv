
namespace LM.Core.Application
{
    public interface IRestService
    {
        void Post(string endPoint, object content);
    }
}
