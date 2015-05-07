using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.Application
{
    public interface ITemplateMensagemAplicacao
    {
        TemplateMensagem ObterPorTipoTemplate(TipoTemplateMensagem tipo);
    }

    public class TemplateMensagemAplicacao : ITemplateMensagemAplicacao
    {
        private readonly IRepositorioTemplateMensagem _repositorio;
        public TemplateMensagemAplicacao(IRepositorioTemplateMensagem repositorio)
        {
            _repositorio = repositorio;
        }

        public TemplateMensagem ObterPorTipoTemplate(TipoTemplateMensagem tipo)
        {
            return _repositorio.ObterPorTipoTemplate(tipo);
        }
    }
}
