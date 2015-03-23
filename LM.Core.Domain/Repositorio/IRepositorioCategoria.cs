using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioCategoria
    {
        IList<Categoria> Listar(int secaoId);
    }
}
