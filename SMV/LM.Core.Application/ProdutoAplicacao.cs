﻿using System.Collections.ObjectModel;
using System.Linq;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface IProdutoAplicacao
    {
        Produto Criar(Produto produto, long usuarioId);
        IEnumerable<Produto> ListarPorCategoria(long pontoDemandaId, int categoriaId);
        IEnumerable<Produto> Buscar(long pontoDemandaId, string termo);
    }

    public class ProdutoAplicacao : IProdutoAplicacao
    {
        private readonly IRepositorioProduto _repositorio;
        public ProdutoAplicacao(IRepositorioProduto repositorio)
        {
            _repositorio = repositorio;
        }

        public Produto Criar(Produto produto, long usuarioId)
        {
            if(produto.Categorias == null) produto.Categorias = new Collection<Categoria>();
            if (!produto.Categorias.Any()) produto.Categorias.Add(new Categoria { Id = 3 });
            produto = _repositorio.Criar(produto, usuarioId);
            _repositorio.Salvar();
            return produto;
        }

        public IEnumerable<Produto> ListarPorCategoria(long pontoDemandaId, int categoriaId)
        {
            return _repositorio.ListarPorCategoria(categoriaId).SomenteProdutosDoCatalogoOuDoPontoDeDemanda(pontoDemandaId).OrdenadoPorNome();
        }

        public IEnumerable<Produto> Buscar(long pontoDemandaId, string termo)
        {
            return _repositorio.Buscar(termo).SomenteProdutosDoCatalogoOuDoPontoDeDemanda(pontoDemandaId).OrdenadoPorSecao().OrdenadoPorNome();
        }
    }
}
