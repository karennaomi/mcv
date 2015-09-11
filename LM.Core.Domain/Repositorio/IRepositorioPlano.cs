using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioPlano
    {
        IList<Plano> Listar();
    }
}
