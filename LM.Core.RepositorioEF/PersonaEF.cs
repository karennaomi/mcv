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
            return _contexto.Personas.OrderBy(p => p.Ordem).ToList();
        }
    }
}
