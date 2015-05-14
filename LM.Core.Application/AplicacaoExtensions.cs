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
    }
}
