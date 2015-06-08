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
        IEnumerable<IItem> ListarSugestaoPorCategoria(long pontoDemandaI, int categoriaId);
        Compra Obter(long pontoDemandaId, long id);
        Compra Criar(Compra compra);
    }

    public class CompraAplicacao : ICompraAplicacao
    {
        private readonly IRepositorioCompra _compraRepo;
        private readonly IPedidoAplicacao _appPedido;
        private readonly IListaAplicacao _appLista;
        private readonly INotificacaoAplicacao _appNotificacao;
        public CompraAplicacao(IRepositorioCompra compraRepo, IPedidoAplicacao appPedido, IListaAplicacao appLista, INotificacaoAplicacao appNotificacao)
        {
            _compraRepo = compraRepo;
            _appPedido = appPedido;
            _appLista = appLista;
            _appNotificacao = appNotificacao;
        }

        public IEnumerable<IItem> ListarSugestao(long pontoDemandaId)
        {
            IEnumerable<IItem> pedidos = _appPedido.ListarItensPorStatus(pontoDemandaId, StatusPedido.Pendente);
            IEnumerable<IItem> itens = _appLista.ListarItens(pontoDemandaId).Where(i => i.EhSugestaoDeCompra);
            return pedidos.Union(itens).OrderBy(i => i.Produto.Categorias.First().CategoriaPai.Nome);
        }

        public IEnumerable<IItem> ListarSugestaoPorCategoria(long pontoDemandaId, int categoriaId)
        {
            if (categoriaId == 0) return _appPedido.ListarItensPorStatus(pontoDemandaId, StatusPedido.Pendente);
            return _appLista.ListarItens(pontoDemandaId).Where(i => i.EhSugestaoDeCompra && i.Produto.Categorias.Any(c => c.CategoriaPai.Id == categoriaId));
        }

        public IList<Categoria> ListarSecoes(long pontoDemandaId)
        {
            var secoes = _appLista.ListarItens(pontoDemandaId).Where(i => i.EhSugestaoDeCompra).ListarSecoes();
            secoes.Insert(0, new Categoria { Id = 0, Nome = "PEDIDO" });
            return secoes;
        }

        public Compra Obter(long pontoDemandaId, long id)
        {
            return _compraRepo.Obter(pontoDemandaId, id);
        }

        public Compra Criar(Compra compra)
        {
            compra.Validar();
            compra = _compraRepo.Criar(compra);
            _appNotificacao.NotificarIntegrantesDoPontoDamanda(compra.Integrante, compra.PontoDemanda, TipoTemplateMensagem.FinalizarCompra, new { Action = "compras" });
            return compra;
        }

        
    }
}
