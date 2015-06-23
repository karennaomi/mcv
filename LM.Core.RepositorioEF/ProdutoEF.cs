using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class ProdutoEF : IRepositorioProduto
    {
        private readonly ContextoEF _contexto;
        public ProdutoEF()
        {
            _contexto = new ContextoEF();
        }

        public ProdutoEF(ContextoEF contexto)
        {
            _contexto = contexto;
        }

        public Produto Criar(Produto produto)
        {
            var categorias = new Categoria[produto.Categorias.Count];
            produto.Categorias.CopyTo(categorias, 0);
            produto.Categorias.Clear();
            foreach (var categoria in categorias)
            {
                produto.Categorias.Add(_contexto.Categorias.Single(c => c.Id == categoria.Id));
            }
            produto = _contexto.Produtos.Add(produto);
            return produto;
        }

        public IEnumerable<Produto> ListarPorCategoria(int categoriaId)
        {
            return _contexto.Produtos.AsNoTracking().Where(p => p.Categorias.Any(c => c.Id == categoriaId) && p.Ativo);
        }
        
        public IEnumerable<Produto> Buscar(string termo)
        {
            var searchFts = FtsInterceptor.Fts(termo);
            return _contexto.Produtos.AsNoTracking().Where(p => p.Ean.Contains(searchFts) || p.Info.Nome.Contains(searchFts) || p.Info.Marca.Contains(searchFts) && p.Ativo);
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}
