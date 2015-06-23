using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioProduto
    {
        Produto Criar(Produto produto);
        IEnumerable<Produto> ListarPorCategoria(long usuarioId, int categoriaId);
        IEnumerable<Produto> Buscar(long usuarioId, string search);
        void Salvar();
    }
}
