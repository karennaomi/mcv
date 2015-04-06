using System.Data.Entity;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class ProdutoEF : IRepositorioProduto
    {
        private readonly IUnitOfWork<ContextoEF> _uniOfWork;
        public ProdutoEF(IUnitOfWork<ContextoEF> uniOfWork)
        {
            _uniOfWork = uniOfWork;
        }

        public Produto Criar(Produto produto)
        {
            foreach (var categoria in produto.Categorias)
            {
                _uniOfWork.Contexto.Entry(categoria).State = EntityState.Unchanged;
            }
            produto = _uniOfWork.Contexto.Produtos.Add(produto);
            _uniOfWork.Contexto.SaveChanges();
            return produto;
        }

        public IEnumerable<Produto> ListarPorCategoria(int categoriaId)
        {
            return _uniOfWork.Contexto.Produtos.Where(p => p.Categorias.Any(c => c.Id == categoriaId));
        }
    }
}
