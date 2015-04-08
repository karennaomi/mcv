using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.Application
{
    public interface ITemplateMensagemAplicacao
    {
        T ObterPorTipo<T>(TipoTemplateMensagem tipo) where T : TemplateMensagem;
    }

    public class TemplateMensagemAplicacao : ITemplateMensagemAplicacao
    {
        private readonly IRepositorioTemplateMensagem _repositorio;
        public TemplateMensagemAplicacao(IRepositorioTemplateMensagem repositorio)
        {
            _repositorio = repositorio;
        }

        public T ObterPorTipo<T>(TipoTemplateMensagem tipo) where T : TemplateMensagem
        {
            return _repositorio.ObterPorTipo<T>(tipo);
        }
    }
}
