using System.Linq;
using LM.Core.Domain;
using LM.Core.Domain.CustomException;
using LM.Core.Domain.Repositorio;
using System.Data.Entity;
using System.Collections.Generic;

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
            integrante = _contexto.Integrantes.Add(integrante);
            foreach (var grupoDeIntegrantes in integrante.GruposDeIntegrantes)
            {
                _contexto.Entry(grupoDeIntegrantes.PontoDemanda).State = EntityState.Unchanged;
            }
            _contexto.SaveChanges();
            return integrante;
        }

        public void VerificarSeCpfJaExiste(string cpf)
        {
            if (_contexto.Integrantes.AsNoTracking().Any(i => i.Cpf == cpf)) throw new IntegranteExistenteException("Cpf", "Cpf");
        }

        public void VerificarSeEmailJaExiste(string email)
        {
            if (_contexto.Integrantes.AsNoTracking().Any(i => i.Email == email)) throw new IntegranteExistenteException("Email", "E-mail");
        }

        public void Remover(Integrante integrante)
        {
            _contexto.Integrantes.Remove(integrante);
        }

        public void RemoverGrupo(Integrante integrante, long pontoDemandaId)
        {
            var grupoIntegrante = integrante.GruposDeIntegrantes.FirstOrDefault(g => g.PontoDemanda.Id == pontoDemandaId);
            _contexto.Set<GrupoDeIntegrantes>().Remove(grupoIntegrante);
        }

        public void Salvar()
        {
            _contexto.SaveChanges();
        }

        public IEnumerable<Animal> Animais()
        {
            return _contexto.Animais.Where(a => a.Ativo);
        }
    }
}
