using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioProduto
    {
        Produto Criar(Produto produto);
        IEnumerable<Produto> ListarPorCategoria(long usuarioId, long pontoDemandaId, int categoriaId);
        IEnumerable<Produto> Buscar(long usuarioId, long pontoDemandaId, string search);
        void Salvar();
    }
}
