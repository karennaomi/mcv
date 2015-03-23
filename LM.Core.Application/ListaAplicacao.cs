using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;

namespace LM.Core.Application
{
    public interface IListaAplicacao : IRelacionaPontoDemanda
    {
        ListaItem AdicionarItem(ListaItem item);
        void RemoverItem(long id);
        IList<Categoria> ListarSecoes();
        IEnumerable<ListaItem> ListarItensPorCategoria(int categoriaId);
        void AtualizarEstoqueDoItem(long itemId, decimal quantidade);
        void AtualizarConsumoDoItem(long itemId, decimal quantidade);
        void AtualizarPeriodoDoItem(long itemId, int periodoId);
    }

    public class ListaAplicacao : RelacionaPontoDemanda, IListaAplicacao
    {
        private readonly IRepositorioLista _repositorio;
        public ListaAplicacao(IRepositorioLista repositorio, long pontoDemandaId) : base(pontoDemandaId)
        {
            _repositorio = repositorio;
        }

        public ListaItem AdicionarItem(ListaItem item)
        {
            return _repositorio.AdicionarItem(PontoDemandaId, item);
        }

        public void RemoverItem(long id)
        {
            _repositorio.RemoverItem(PontoDemandaId, id);
        }

        public IList<Categoria> ListarSecoes()
        {
            return _repositorio.ListarSecoes(PontoDemandaId);
        }

        public IEnumerable<ListaItem> ListarItensPorCategoria(int categoriaId)
        {
            return _repositorio.ListarItensPorCategoria(PontoDemandaId, categoriaId);
        }

        public void AtualizarEstoqueDoItem(long itemId, decimal quantidade)
        {
            _repositorio.AtualizarEstoqueDoItem(PontoDemandaId, itemId, quantidade);
        }

        public void AtualizarConsumoDoItem(long itemId, decimal quantidade)
        {
            _repositorio.AtualizarConsumoDoItem(PontoDemandaId, itemId, quantidade);
        }

        public void AtualizarPeriodoDoItem(long itemId, int periodoId)
        {
            _repositorio.AtualizarPeriodoDoItem(PontoDemandaId, itemId, periodoId);
        }
    }
}



//private readonlListaRepositorioEFDO _repositorio;

//public ListaAplicacao()
//{
//    _repositorio = neListaRepositorioEFDO();
//}

//public void SalvarLista(Lista lista)
//{
//    if (lista.IdLista > 0)

//    {
//        lista.DtAlteracao = DateTime.Parse(String.Format("{0:d/M/yyyy HH:mm:ss}", DateTime.Now));
//    }
//    else
//    {
//        lista.DtInclusao = DateTime.Now;
//    }
//    _repositorio.SalvarLista(lista);
//}

//public IEnumerable<Lista> ExibirListas()
//{
//    return _repositorio.ListarListas(9999);
//}


//public Lista ListaPorId(int idLista)
//{
//    return _repositorio.ListPorId(idLista, 9999);
//}

//public Lista CriarListaConsumo(int pontoDemandaId)
//{
//    return _repositorio.CriarListaConsumo(pontoDemandaId, 9999);
//}

//public Lista BuscarListaConsumo(int pontoDemandaId)
//{
//    return _repositorio.BuscarListaConsumo(pontoDemandaId, 9999);
//}