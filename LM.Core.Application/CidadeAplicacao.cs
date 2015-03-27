using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.Application
{
    public interface ICidadeAplicacao
    {
        Cidade Buscar(string nome);
    }

    public class CidadeAplicacao : ICidadeAplicacao
    {
        private readonly IRepositorioCidade _repositorio;
        public CidadeAplicacao(IRepositorioCidade repositorio)
        {
            _repositorio = repositorio;
        }

        public Cidade Buscar(string nome)
        {
            return _repositorio.Buscar(nome);
        }
    }
}
