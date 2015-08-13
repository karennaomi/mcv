using LM.Core.Domain;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.Application
{
    public static class AplicacaoExtensions
    {
        public static IList<Categoria> ListarSecoes(this IEnumerable<IItem> itens)
        {
            return
                itens.Select(i => i.Produto.Categorias.Select(c => c.CategoriaPai).First())
                    .Distinct()
                    .OrderBy(c => c.Nome)
                    .ToList();
        }

        public static IOrderedEnumerable<Produto> OrdenadoPorSecao(this IEnumerable<Produto> produtos)
        {
            return produtos.OrderBy(p => p.Categorias.First().CategoriaPai.Nome).ThenBy(p => p.Categorias.First().Nome);
        }

        public static IOrderedEnumerable<T> OrdenadoPorSecao<T>(this IEnumerable<T> itens) where T : IItem
        {
            return itens.OrderBy(i => i.Produto.Categorias.First().CategoriaPai.Nome).ThenBy(i => i.Produto.Categorias.First().Nome);
        }

        public static IEnumerable<T> DaSecao<T>(this IEnumerable<T> itens, int secaoId) where T : IItem
        {
            return secaoId == 0 ? itens : itens.Where(i => i.Produto.Categorias.Any(c => c.CategoriaPai.Id == secaoId));
        }

        public static IEnumerable<PedidoItem> NaoExluidoPeloUsuario(this IEnumerable<PedidoItem> itens)
        {
            return itens.Where(i => i.Status != StatusPedido.ExcluidoPeloUsuario);
        }

        public static IEnumerable<PedidoItem> DoStatus(this IEnumerable<PedidoItem> itens, StatusPedido status)
        {
            return itens.Where(i => i.Status == status);
        }

        public static IOrderedEnumerable<PedidoItem> OrdenadoPorStatus(this IEnumerable<PedidoItem> itens)
        {
            return itens.OrderBy(i => i.Status);
        }

        public static IEnumerable<Produto> SomenteProdutosDoCatalogoOuDoPontoDeDemanda(this IEnumerable<Produto> produtos, long pontoDemandaId)
        {
            return produtos.Where(Produto.ProtectProductPredicate(pontoDemandaId));
        }

        public static bool JaExisteProdutoNaLista(this Lista lista, ListaItem item)
        {
            return lista.Itens.Any(i => i.Produto.Id == item.Produto.Id && i.Status == "A");
        }

        public static IEnumerable<ListaItem> SomenteAtivos(this IEnumerable<ListaItem> itens)
        {
            return itens.Where(i => i.Status == "A");
        }
    }
}
