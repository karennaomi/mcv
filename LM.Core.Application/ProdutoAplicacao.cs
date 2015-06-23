using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface IProdutoAplicacao
    {
        Produto Criar(Produto produto);
        IEnumerable<Produto> ListarPorCategoria(long usuarioId, int categoriaId);
        IEnumerable<Produto> Buscar(long usuarioId, string termo);
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
            produto = _repositorio.Criar(produto);
            _repositorio.Salvar();
            return produto;
        }

        public IEnumerable<Produto> ListarPorCategoria(long usuarioId, int categoriaId)
        {
            return _repositorio.ListarPorCategoria(usuarioId, categoriaId);
        }

        public IEnumerable<Produto> Buscar(long usuarioId, string termo)
        {
            return _repositorio.Buscar(usuarioId, termo);
        }
    }
}
