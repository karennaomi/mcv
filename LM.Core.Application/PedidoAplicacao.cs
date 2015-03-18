﻿using LM.Core.Domain;
using LM.Core.Repository;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface IPedidoAplicacao : IRelacionaPontoDemanda
    {
        IEnumerable<PedidoItem> ListarItensPorCategoria(int categoriaId);
        IList<Categoria> ListarSecoes(StatusPedido status);
        void RemoverItem(long itemId);
        void AtualizarQuantidadeDoItem(long itemId, decimal quantidade);
        PedidoItem AdicionarItem(PedidoItem item);
    }

    public class PedidoAplicacao : RelacionaPontoDemanda, IPedidoAplicacao
    {
        private readonly IRepositorioPedido _repositorio;
        public PedidoAplicacao(IRepositorioPedido repositorio, long pontoDemandaId) : base(pontoDemandaId)
        {
            _repositorio = repositorio;
        }

        public IEnumerable<PedidoItem> ListarItensPorCategoria(int categoriaId)
        {
            return _repositorio.ListarItensPorCategoria(PontoDemandaId, categoriaId);
        }

        public IList<Categoria> ListarSecoes(StatusPedido status)
        {
            return _repositorio.ListarSecoes(PontoDemandaId, status);
        }

        public void RemoverItem(long itemId)
        {
            _repositorio.RemoverItem(PontoDemandaId, itemId);
        }

        public void AtualizarQuantidadeDoItem(long itemId, decimal quantidade)
        {
            _repositorio.AtualizarQuantidadeDoItem(PontoDemandaId, itemId, quantidade);
        }

        public PedidoItem AdicionarItem(PedidoItem item)
        {
            return _repositorio.AdicionarItem(PontoDemandaId, item);
        }
    }
}
