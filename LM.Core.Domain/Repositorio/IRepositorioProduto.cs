using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioProduto
    {
        Produto Criar(Produto produto, long usuarioId);
        IEnumerable<Produto> ListarPorCategoria(int categoriaId);
        IEnumerable<Produto> Buscar(string search);
        void Salvar();
    }
}
