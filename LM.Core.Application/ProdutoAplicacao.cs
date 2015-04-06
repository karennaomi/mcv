using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface IProdutoAplicacao
    {
        Produto Criar(Produto produto);
        IEnumerable<Produto> ListarPorCategoria(int categoriaId);
    }

    public class ProdutoAplicacao : IProdutoAplicacao
    {
        private readonly IRepositorioProduto _repositorio;
        public ProdutoAplicacao(IRepositorioProduto repositorio)
        {
            _repositorio = repositorio;
        }

        public Produto Criar(Produto produto)
        {
            return _repositorio.Criar(produto);
        }

        public IEnumerable<Produto> ListarPorCategoria(int categoriaId)
        {
            return _repositorio.ListarPorCategoria(categoriaId);
        }
    }
}
