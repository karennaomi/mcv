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

        public static IEnumerable<T> OrderBySecoes<T>(this IEnumerable<T> itens) where T : IItem
        {
            return itens.OrderBy(i => i.Produto.Categorias.First().CategoriaPai.Nome).ThenBy(i => i.Produto.Categorias.First().Nome);
        }
    }
}
