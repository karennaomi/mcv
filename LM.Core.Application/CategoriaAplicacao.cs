using LM.Core.Domain;
using LM.Core.Repository;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface ICategoriaAplicacao
    {
        IList<Categoria> Listar(int secaoId);
    }

    public class CategoriaAplicacao : ICategoriaAplicacao
    {
        private readonly IRepositorioCategoria _repositorio;
        public CategoriaAplicacao(IRepositorioCategoria repositorio)
        {
            _repositorio = repositorio;
        }

        public IList<Categoria> Listar(int secaoId)
        {
            return _repositorio.Listar(secaoId);
        }
    }
}
