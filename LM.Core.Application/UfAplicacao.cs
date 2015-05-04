using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface IUfAplicacao
    {
        IList<Uf> Listar();
    }

    public class UfAplicacao : IUfAplicacao
    {
        private readonly IRepositorioUf _repositorio;
        public UfAplicacao(IRepositorioUf repositorio)
        {
            _repositorio = repositorio;
        }

        public IList<Uf> Listar()
        {
            return _repositorio.Listar();
        }
    }
}
