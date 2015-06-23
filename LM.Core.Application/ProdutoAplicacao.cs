using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface IProdutoAplicacao
    {
        Produto Criar(Produto produto);
        IEnumerable<Produto> ListarPorCategoria(long usuarioId, long pontoDemandaId, int categoriaId);
        IEnumerable<Produto> Buscar(long usuarioId, long pontoDemandaId, string termo);
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

        public IEnumerable<Produto> ListarPorCategoria(long usuarioId, long pontoDemandaId, int categoriaId)
        {
            return _repositorio.ListarPorCategoria(usuarioId, pontoDemandaId, categoriaId);
        }

        public IEnumerable<Produto> Buscar(long usuarioId, long pontoDemandaId, string termo)
        {
            return _repositorio.Buscar(usuarioId, pontoDemandaId, termo);
        }
    }
}
