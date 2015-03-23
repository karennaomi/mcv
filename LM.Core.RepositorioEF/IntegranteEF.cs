namespace LM.Core.RepositorioEF
{
    public class IntegranteEF : IRepositorioIntegrante
    {
        private readonly ContextoEF _contexto;
        public IntegranteEF()
        {
            _contexto = new ContextoEF();
        }

        public Integrante Criar(Integrante integrante)
        {
            _contexto.Entry(integrante.GrupoDeIntegrantes).State = EntityState.Unchanged;
            _contexto.Entry(integrante.Persona).State = EntityState.Unchanged;
            integrante = _contexto.Integrantes.Add(integrante);
            _contexto.SaveChanges();
            return integrante;
        }

        public void Apagar(long id)
        {
            var integrante = _contexto.Integrantes.Find(id);
            _contexto.Entry(integrante).State = EntityState.Deleted;
            _contexto.Integrantes.Remove(integrante);
            _contexto.SaveChanges();
        }
    }
}
