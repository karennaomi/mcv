using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioProduto
    {
        Produto Criar(Produto produto);
        IEnumerable<Produto> ListarPorCategoria(int categoriaId);
        void Salvar();
    }
}
