﻿using System;
using System.Collections.ObjectModel;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.Application
{
    public interface ICompraAplicacao
    {
        IList<Categoria> ListarSecoes(long pontoDemandaId);
        IEnumerable<IItem> ListarSugestao(long pontoDemandaId);
        Compra Obter(long pontoDemandaId, long id);
        Compra Criar(Compra compra);
        IEnumerable<MotivoSubstituicao> MotivosSubstituicao();
        IEnumerable<CompraItem> ListarItensSubstitutos(long pontoDemandaId, long itemId);
    }

    public class CompraAplicacao : ICompraAplicacao
    {
        private readonly IRepositorioCompra _compraRepo;
        private readonly IPedidoAplicacao _appPedido;
        private readonly IListaAplicacao _appLista;
        private readonly ICompraAtivaAplicacao _appCompraAtiva;
        public CompraAplicacao(IRepositorioCompra compraRepo, IPedidoAplicacao appPedido, IListaAplicacao appLista, ICompraAtivaAplicacao appCompraAtiva)
        {
            _compraRepo = compraRepo;
            _appPedido = appPedido;
            _appLista = appLista;
            _appCompraAtiva = appCompraAtiva;
        }

        public IEnumerable<IItem> ListarSugestao(long pontoDemandaId)
        {
            IEnumerable<IItem> pedidos = _appPedido.ListarItensPorStatus(pontoDemandaId, StatusPedido.Pendente);
            IEnumerable<IItem> itens = _appLista.ListarItens(pontoDemandaId).Where(i => i.EhSugestaoDeCompra);
            return pedidos.Union(itens);
        }

        public IList<Categoria> ListarSecoes(long pontoDemandaId)
        {
            IEnumerable<IItem> pedidos = _appPedido.ListarItens(pontoDemandaId);
            IEnumerable<IItem> itens = _appLista.ListarItens(pontoDemandaId).Where(i => i.EhSugestaoDeCompra);
            return pedidos.Union(itens).ListarSecoes();
        }

        public Compra Obter(long pontoDemandaId, long id)
        {
            return _compraRepo.Obter(pontoDemandaId, id);
        }

        public Compra Criar(Compra compra)
        {
            if(compra.PontoDemanda.Listas == null) compra.PontoDemanda.Listas = new Collection<Lista> {_appLista.ObterListaPorPontoDemanda(compra.PontoDemanda.Id)};
            if(!_appCompraAtiva.ExisteCompraAtiva(compra.PontoDemanda.Id)) throw new ApplicationException("Não existe uma compra ativa.");
            compra.Validar();
            compra = _compraRepo.Criar(compra);
            AtualizarStatusItensPedido(compra.Itens.OfType<PedidoCompraItem>());
            _compraRepo.PreencherProdutoId(compra.Itens); //Campo desnecessário no modelo, mas quem fez não sabe dizer onde é usado ou mudar para usar o produto associado ao item se necessário
            _compraRepo.Salvar();
            _appCompraAtiva.FinalizarCompra(compra.PontoDemanda.Id);
            _compraRepo.LancarEstoque(compra);
            _compraRepo.PreencheTabelaRelacionamentoCompraPedido(compra.Itens.OfType<PedidoCompraItem>()); //Aqui o id do pedido foi movido para a propria tabela de item da compra  mas como não sabem onde mudar nas procs preencho essa tabela pra não quebrar procs
            _compraRepo.RecalcularSugestao(compra.PontoDemanda.Id);
            return compra;
        }

        public IEnumerable<MotivoSubstituicao> MotivosSubstituicao()
        {
            return _compraRepo.MotivosSubstituicao();
        }

        public IEnumerable<CompraItem> ListarItensSubstitutos(long pontoDemandaId, long itemId)
        {
            var compras = _compraRepo.Listar(pontoDemandaId).ToList();
            var compraItens = new List<CompraItem>();
            foreach (var listaCompraItems in compras.Select(c => c.Itens.OfType<ListaCompraItem>().Where(i => i.ItemSubstituto != null && ObterItemId(i.ItemSubstituto.Original) == itemId)))
            {
                compraItens.AddRange(listaCompraItems);    
            }
            foreach (var listaCompraItems in compras.Select(c => c.Itens.OfType<PedidoCompraItem>().Where(i => i.ItemSubstituto != null && ObterItemId(i.ItemSubstituto.Original) == itemId)))
            {
                compraItens.AddRange(listaCompraItems);
            }
            return compraItens;
        }

        private long ObterItemId(CompraItem compraItem)
        {
            if (compraItem is ListaCompraItem) return ((ListaCompraItem) compraItem).Item.Id;
            else if (compraItem is PedidoCompraItem) return ((PedidoCompraItem)compraItem).Item.Id;
            else
            {
                throw new ApplicationException("Tipo do item de compra inválido");
            }
        }



        private static void AtualizarStatusItensPedido(IEnumerable<PedidoCompraItem> itens)
        {
            foreach (var pedidoCompraItem in itens)
            {
                pedidoCompraItem.Item.Status = GetStatus(pedidoCompraItem.Status);
            }
        }

        private static StatusPedido GetStatus(StatusCompra statusCompra)
        {
            switch (statusCompra)
            {
                case StatusCompra.ItemAComprar:
                case StatusCompra.NaoEncontrado:
                    return StatusPedido.Pendente;
                case StatusCompra.Comprado:
                    return StatusPedido.Comprado;
                case StatusCompra.AgoraNao:
                    return StatusPedido.Rejeitado;
                case StatusCompra.ItemSubstituido:
                    return StatusPedido.Substituido;
                default: return StatusPedido.Pendente;
            }
        }
    }
}
