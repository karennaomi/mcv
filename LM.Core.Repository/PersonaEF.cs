using LM.Core.Domain;
using LM.Core.Repository.EntityFramework;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.Repository
{
    public interface IRepositorioPersona
    {
        IList<Persona> Listar();
    }

    public class PersonaEF : IRepositorioPersona
    {
        private readonly ContextoEF _contexto;
        public PersonaEF()
        {
            _contexto = new ContextoEF();
        }

        public IList<Persona> Listar()
        {
            return _contexto.Personas.OrderBy(p => p.Ordem).ToList();
        }
    }
}
