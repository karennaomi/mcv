using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections;
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
    }

    public class CompraAplicacao : ICompraAplicacao
    {
        private readonly IRepositorioCompra _compraRepo;
        private readonly IPedidoAplicacao _appPedido;
        private readonly IListaAplicacao _appLista;
        public CompraAplicacao(IRepositorioCompra compraRepo, IPedidoAplicacao appPedido, IListaAplicacao appLista)
        {
            _compraRepo = compraRepo;
            _appPedido = appPedido;
            _appLista = appLista;
        }

        public IEnumerable<IItem> ListarSugestao(long pontoDemandaId)
        {
            IEnumerable<IItem> pedidos = _appPedido.ListarItensPorStatus(pontoDemandaId, StatusPedido.Pendente);
            IEnumerable<IItem> itens = _appLista.ListarItens(pontoDemandaId).Where(i => i.EhSugestaoDeCompra);
            return pedidos.Union(itens).OrderBy(i => i.Produto.Categorias.First().CategoriaPai.Nome);
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
            return _compraRepo.Criar(compra);
        }

        
    }
}
