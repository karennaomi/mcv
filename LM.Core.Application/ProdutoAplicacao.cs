using LM.Core.Domain;
using LM.Core.Repository;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface IProdutoAplicacao
    {
        IEnumerable<Produto> ListarPorCategoria(int categoriaId);
    }

    public class ProdutoAplicacao : IProdutoAplicacao
    {
        private readonly IRepositorioProduto _repositorio;
        public ProdutoAplicacao(IRepositorioProduto repositorio)
        {
            _repositorio = repositorio;
        }     
    
        public IEnumerable<Produto> ListarPorCategoria(int categoriaId)
        {
            return _repositorio.ListarPorCategoria(categoriaId);
        }
    }
}
