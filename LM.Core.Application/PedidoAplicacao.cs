using System;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.Application
{
    public interface IPedidoAplicacao
    {
        IEnumerable<PedidoItem> ListarItens(long pontoDemandaId);
        IEnumerable<PedidoItem> ListarItensPorCategoria(long pontoDemandaId, int categoriaId);
        IEnumerable<PedidoItem> ListarItensPorStatus(long pontoDemandaId, StatusPedido status);
        IList<Categoria> ListarSecoes(long pontoDemandaId, StatusPedido status);
        void RemoverItem(long pontoDemandaId, long usuarioId, long itemId);
        void AtualizarQuantidadeDoItem(long pontoDemandaId, long usuarioId, long itemId, decimal quantidade);
        PedidoItem AdicionarItem(long pontoDemandaId, PedidoItem item);
    }

    public class PedidoAplicacao : IPedidoAplicacao
    {
        private readonly IRepositorioPedido _repositorio;
        private readonly ICompraAtivaAplicacao _appCompraAtiva;
        private readonly INotificacaoAplicacao _appNotificacao;

        public PedidoAplicacao(IRepositorioPedido repositorio, ICompraAtivaAplicacao appCompraAtiva, INotificacaoAplicacao appNotificacao)
        {
            _repositorio = repositorio;
            _appCompraAtiva = appCompraAtiva;
            _appNotificacao = appNotificacao;
        }

        public IEnumerable<PedidoItem> ListarItens(long pontoDemandaId)
        {
            return _repositorio.ListarItens(pontoDemandaId).Where(i => i.Status != StatusPedido.ExcluidoPeloUsuario).OrderBySecoes().ThenBy(i => i.Status);
        }

        public IEnumerable<PedidoItem> ListarItensPorCategoria(long pontoDemandaId, int categoriaId)
        {
            var itens = _repositorio.ListarItens(pontoDemandaId);
            return itens.Where(i => i.Produto.Categorias.Any(c => c.CategoriaPai.Id == categoriaId) && i.Status != StatusPedido.ExcluidoPeloUsuario);
        }

        public IEnumerable<PedidoItem> ListarItensPorStatus(long pontoDemandaId, StatusPedido status)
        {
            var itens = _repositorio.ListarItens(pontoDemandaId);
            return itens.Where(i => i.Status == status);
        }

        public IList<Categoria> ListarSecoes(long pontoDemandaId, StatusPedido status)
        {
            var itens = _repositorio.ListarItens(pontoDemandaId).Where(i => i.Status == status);
            return itens.Select(i => i.Produto.Categorias.Select(c => c.CategoriaPai).First()).Distinct().OrderBy(c => c.Nome).ToList();
        }

        public void RemoverItem(long pontoDemandaId, long usuarioId, long itemId)
        {
            var itens = _repositorio.ListarItens(pontoDemandaId);
            var item = ObterItem(itens, itemId);
            if (item.Integrante.Usuario.Id != usuarioId) throw new ApplicationException("Somente quem criou o item pode remove-lo.");
            item.Status = StatusPedido.ExcluidoPeloUsuario;
            item.DataAlteracao = DateTime.Now;
            _repositorio.Salvar();
        }

        public void AtualizarQuantidadeDoItem(long pontoDemandaId, long usuarioId, long itemId, decimal quantidade)
        {
            var item = ObterItem(_repositorio.ListarItens(pontoDemandaId), itemId);
            if (item.Integrante.Usuario.Id != usuarioId) throw new ApplicationException("Somente quem criou o item pode alterá-lo.");
            item.Quantidade = quantidade;
            item.DataAlteracao = DateTime.Now;
            _repositorio.Salvar();
        }

        public PedidoItem AdicionarItem(long pontoDemandaId, PedidoItem item)
        {
            item = _repositorio.AdicionarItem(pontoDemandaId, item);
            if (!_appCompraAtiva.ExisteCompraAtiva(pontoDemandaId)) return item;
            var compraAtiva = _appCompraAtiva.Obter(pontoDemandaId);
            _appNotificacao.Notificar(item.Integrante, compraAtiva.Usuario.Integrante, item.PontoDemanda, TipoTemplateMensagem.PedidoItemCriado, new {Action = "pedidos"});
            return item;
        }

        private static PedidoItem ObterItem(IEnumerable<PedidoItem> itens, long id)
        {
            var item = itens.SingleOrDefault(i => i.Id == id);
            if (item == null) throw new ApplicationException("O pedido não possui o item informado.");
            return item;
        }
    }
}
