using System.Data.Entity;
using LM.Core.Domain;
using LM.Core.Repository.EntityFramework;

namespace LM.Core.Repository
{
    public interface IRepositorioIntegrante
    {
        Integrante Criar(Integrante integrante);
        void Apagar(long id);
    }

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
