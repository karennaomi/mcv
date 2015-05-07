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

        public TemplateMensagem ObterPorTipoTemplate(TipoTemplateMensagem tipo)
        {
            return _contexto.TemplatesMensagens.AsNoTracking().FirstOrDefault(t => t.Tipo == tipo);
        }
    }
}
