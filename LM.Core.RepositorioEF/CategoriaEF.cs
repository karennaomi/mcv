using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class CategoriaEF : IRepositorioCategoria
    {
        private readonly ContextoEF _contextoEF;
        public CategoriaEF()
        {
            _contextoEF = new ContextoEF();
        }

        public IList<Categoria> Listar(int secaoId)
        {
            return _contextoEF.Categorias.Where(c => c.CategoriaPai.Id == secaoId).OrderBy(c => c.Nome).ToList();
        }
    }
}
