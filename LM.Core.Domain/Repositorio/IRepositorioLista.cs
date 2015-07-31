using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioLista
    {
        Lista ObterListaPorPontoDemanda(long pontoDemandaId);
        ListaItem AdicionarItem(Lista lista, ListaItem item, long usuarioId);
        void AtualizarItem(ListaItem item, int periodoId, long usuarioId);
        IEnumerable<ListaItem> BuscarItens(Lista lista, long pontoDemandaId, string termo);
        void LancarEstoque(long pontoDemandaId, long usuarioId, int? produtoId, decimal? quantidade);
        IEnumerable<Periodo> PeriodosDeConsumo();
        void Salvar();
    }
}
