using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioLista
    {
        Lista ObterListaPorPontoDemanda(long pontoDemandaId);
        ListaItem AdicionarItem(Lista lista, ListaItem item, long usuarioId);
        void AtualizarPeriodoDoItem(ListaItem item, int periodoId);
        IEnumerable<ListaItem> BuscarItens(Lista lista, long pontoDemandaId, string termo);
        void LancarEstoque(long pontoDemandaId, long integranteId, int? produtoId, decimal? quantidade);
        void Salvar();
    }
}
