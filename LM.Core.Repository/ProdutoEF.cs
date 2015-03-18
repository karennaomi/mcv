using System.Linq;
using LM.Core.Domain;
using LM.Core.Repository.EntityFramework;
using System.Collections.Generic;

namespace LM.Core.Repository
{
    public interface IRepositorioProduto
    {
        IEnumerable<Produto> ListarPorCategoria(int categoriaId);
    }

    public class ProdutoEF : IRepositorioProduto
    {
        private readonly ContextoEF _contexto;
        public ProdutoEF()
        {
            _contexto= new ContextoEF();
        }

        public IEnumerable<Produto> ListarPorCategoria(int categoriaId)
        {
            return _contexto.Produtos.Where(p => p.Categorias.Any(c => c.Id == categoriaId));
        }
    }
}
