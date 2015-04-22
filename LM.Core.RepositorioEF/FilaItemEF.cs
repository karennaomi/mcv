using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.RepositorioEF
{
    public class FilaItemEF : IRepositorioFilaItem
    {
        private readonly ContextoEF _contexto;
        public FilaItemEF()
        {
            _contexto = new ContextoEF();
        }

        public FilaItem Criar(FilaItem filaItem)
        {
            filaItem = _contexto.FilaItens.Add(filaItem);
            _contexto.SaveChanges();
            return filaItem;
        }
    }
}
