using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class PersonaEF : IRepositorioPersona
    {
        private readonly ContextoEF _contexto;
        public PersonaEF()
        {
            _contexto = new ContextoEF();
        }

        public IList<Persona> Listar()
        {
            return _contexto.Personas.AsNoTracking().OrderBy(p => p.Ordem).ToList();
        }
    }
}
