﻿using System;
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
            produto = _repositorio.Criar(produto, usuarioId);
            _repositorio.Salvar();
            return produto;
        }

        public IEnumerable<Produto> ListarPorCategoria(long pontoDemandaId, int categoriaId)
        {
            return _repositorio.ListarPorCategoria(categoriaId).Where(Produto.ProtectProductPredicate(pontoDemandaId));
        }

        public IEnumerable<Produto> Buscar(long pontoDemandaId, string termo)
        {
            return _repositorio.Buscar(termo).Where(Produto.ProtectProductPredicate(pontoDemandaId));
        }
    }
}
