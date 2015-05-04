using System.Collections.Generic;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;

namespace LM.Core.Application
{
    public interface ICidadeAplicacao
    {
        Cidade Buscar(string nome);
        IList<Cidade> Listar(int ufId);
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

        public IList<Cidade> Listar(int ufId)
        {
            return _repositorio.Listar(ufId);
        }
    }
}
