using System.Collections.Generic;

namespace LM.Core.Domain.Repositorio
{
    public interface IRepositorioPersona
    {
        IList<Persona> Listar();
    }
}
