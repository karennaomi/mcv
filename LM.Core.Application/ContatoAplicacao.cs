using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.Application
{
    public interface IContatoAplicacao
    {
        Contato Criar(Contato contato);
    }

    public class ContatoAplicacao : IContatoAplicacao
    {
        private readonly IRepositorioContato _repositorio;
        public ContatoAplicacao(IRepositorioContato repositorio)
        {
            _repositorio = repositorio;
        }

        public Contato Criar(Contato contato)
        {
            return _repositorio.Criar(contato);
        }
    }
}
