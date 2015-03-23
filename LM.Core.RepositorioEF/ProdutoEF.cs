namespace LM.Core.RepositorioEF
{
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
