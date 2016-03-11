using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.Application
{
    public interface IFilaItemAplicacao
    {
        FilaItem Criar(FilaItem filaItem);
    }

    public class FilaItemAplicacao : IFilaItemAplicacao
    {
        private readonly IRepositorioFilaItem _repositorio;
        public FilaItemAplicacao(IRepositorioFilaItem repositorio)
        {
            _repositorio = repositorio;
        }

        public FilaItem Criar(FilaItem filaItem)
        {
            return _repositorio.Criar(filaItem);
        }
    }
}
