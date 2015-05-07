using System.Linq;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using System.Data.Entity;

namespace LM.Core.RepositorioEF
{
    public class IntegranteEF : IRepositorioIntegrante
    {
        private readonly ContextoEF _contexto;
        public IntegranteEF()
        {
            _contexto = new ContextoEF();
        }

        public Integrante Obter(long id)
        {
            var integrante = _contexto.Integrantes.Find(id);
            if (integrante == null) throw new ObjetoNaoEncontradoException("Integrante não encontrado, id " + id);
            return integrante;
        }

        public Integrante Criar(Integrante integrante)
        {
            if (integrante.GrupoDeIntegrantes.Id > 0) _contexto.Entry(integrante.GrupoDeIntegrantes).State = EntityState.Unchanged;
            integrante = _contexto.Integrantes.Add(integrante);
            _contexto.SaveChanges();
            return integrante;
        }

        public void Apagar(Integrante integrante)
        {
            _contexto.Entry(integrante).State = EntityState.Deleted;
            _contexto.Integrantes.Remove(integrante);
            _contexto.SaveChanges();
        }

        public void VerificarSeCpfJaExiste(string cpf)
        {
            if (_contexto.Integrantes.AsNoTracking().Any(i => i.Cpf == cpf)) throw new IntegranteExistenteException("Cpf");
        }

        public void VerificarSeEmailJaExiste(string email)
        {
            if (_contexto.Integrantes.AsNoTracking().Any(i => i.Email == email)) throw new IntegranteExistenteException("Email");
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }
    }
}
