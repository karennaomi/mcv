
namespace LM.Core.Application
{
    public interface IRelacionaPontoDemanda
    {
        long PontoDemandaId { get; set; }
    }

    public abstract class RelacionaPontoDemanda : IRelacionaPontoDemanda
    {
        public long PontoDemandaId { get; set; }
        protected RelacionaPontoDemanda(long pontoDemandaId)
        {
            PontoDemandaId = pontoDemandaId;
        }
    }
}
