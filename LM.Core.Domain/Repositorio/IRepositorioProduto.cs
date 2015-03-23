using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioProduto
    {
        IEnumerable<Produto> ListarPorCategoria(int categoriaId);
    }
}
