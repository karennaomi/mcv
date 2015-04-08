using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class TemplateMensagemEF : IRepositorioTemplateMensagem
    {
        private readonly ContextoEF _contextoEF;
        public TemplateMensagemEF()
        {
            _contextoEF = new ContextoEF();
        }

        public T ObterPorTipo<T>(TipoTemplateMensagem tipo) where T : TemplateMensagem
        {
            return _contextoEF.TemplatesMensagens.OfType<T>().FirstOrDefault(t => t.Tipo == tipo);
        }
    }
}
