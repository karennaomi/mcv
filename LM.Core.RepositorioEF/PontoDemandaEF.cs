using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class PontoDemandaEF : IRepositorioPontoDemanda
    {
        private readonly IUnitOfWork<ContextoEF> _uniOfWork;
        public PontoDemandaEF(IUnitOfWork<ContextoEF> uniOfWork)
        {
            _uniOfWork = uniOfWork;
        }

        public IList<PontoDemanda> Listar(long usuarioId)
        {
            return _uniOfWork.Contexto.PontosDemanda.Where(d => d.GrupoDeIntegrantes.Integrantes.Any(i => i.Usuario.Id == usuarioId)).ToList();
        }

        public PontoDemanda Obter(long id, long usuarioId)
        {
            return _uniOfWork.Contexto.PontosDemanda.SingleOrDefault(d => d.GrupoDeIntegrantes.Integrantes.Any(i => i.Usuario.Id == usuarioId) && d.Id == id);
        }

        public void SalvarAlteracoes()
        {
            _uniOfWork.Contexto.SaveChanges();
        }

        public PontoDemanda Criar(PontoDemanda pontoDemanda)
        {
            _uniOfWork.Contexto.Entry(pontoDemanda.GrupoDeIntegrantes).State = EntityState.Modified;
            _uniOfWork.Contexto.Entry(pontoDemanda.Endereco.Cidade).State = EntityState.Unchanged;
            _uniOfWork.Contexto.PontosDemanda.Add(pontoDemanda);
            _uniOfWork.Contexto.SaveChanges();
            return pontoDemanda;
                
        }
    }
}
