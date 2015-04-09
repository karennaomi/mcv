using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class TemplateMensagemEF : IRepositorioTemplateMensagem
    {
        private readonly ContextoEF _contexto;
        public TemplateMensagemEF()
        {
            _contexto = new ContextoEF();
        }

        public T ObterPorTipo<T>(TipoTemplateMensagem tipo) where T : TemplateMensagem
        {
            return _contexto.TemplatesMensagens.AsNoTracking().OfType<T>().FirstOrDefault(t => t.Tipo == tipo);
        }
    }
}
