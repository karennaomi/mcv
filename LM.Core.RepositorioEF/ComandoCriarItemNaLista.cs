﻿using System.Linq;
using LM.Core.Domain;
using System;

namespace LM.Core.RepositorioEF
{
    public class ComandoCriarItemNaLista
    {
        private readonly ContextoEF _contexto;
        private readonly Lista _lista;
        private readonly ListaItem _novoItem;

        public ComandoCriarItemNaLista(ContextoEF contexto, Lista lista, ListaItem novoItem)
        {
            _contexto = contexto;
            _lista = lista;
            _novoItem = novoItem;
        }

        public ListaItem Executar(long usuarioId)
        {
            _novoItem.Produto = ChecarProduto(_novoItem.Produto, usuarioId);
            _novoItem.Periodo = _contexto.Set<Periodo>().Single(p => p.Id == _novoItem.Periodo.Id);
            _novoItem.DataInclusao = DateTime.Now;
            _novoItem.AtualizadoPor = _contexto.Usuarios.Find(usuarioId);
            _lista.Itens.Add(_novoItem);
            _contexto.SaveChanges();
            return _novoItem;
        }

        private Produto ChecarProduto(Produto produto, long usuarioId)
        {
            if (produto.Id != 0) return _contexto.Produtos.Single(p => p.Id == produto.Id);

            var produtoRepo = new ProdutoEF(_contexto);
            return produtoRepo.Criar(produto, usuarioId);
        }
    }
}
