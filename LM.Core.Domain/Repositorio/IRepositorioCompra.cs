
using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioCompra
    {
        IEnumerable<Compra> Listar(long pontoDemandaId);
        Compra Obter(long pontoDemandaId, long id);
        Compra Criar(Compra compra);
        void LancarEstoque(Compra compra);
        void PreencherProdutoId(IEnumerable<CompraItem> itens);
        void PreencheTabelaRelacionamentoCompraPedido(IEnumerable<PedidoCompraItem> itens);
        IEnumerable<MotivoSubstituicao> MotivosSubstituicao();
        void RecalcularSugestao(long pontoDemandaId);
        void Salvar();

        
    }
}
