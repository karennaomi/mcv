using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioLista
    {
        Lista ObterListaPorPontoDemanda(long pontoDemandaId);
        ListaItem AdicionarItem(Lista lista, ListaItem item);
        void AtualizarPeriodoDoItem(ListaItem item, int periodoId);
        IEnumerable<ListaItem> BuscarItens(Lista lista, string termo);
        void Salvar();
    }
}
