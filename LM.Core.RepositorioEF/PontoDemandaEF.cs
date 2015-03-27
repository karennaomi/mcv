using System.Data.Entity;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class PontoDemandaEF : IRepositorioPontoDemanda
    {
        private readonly ContextoEF _contextoEF;
        public PontoDemandaEF()
        {
            _contextoEF = new ContextoEF();
        }

        public IList<PontoDemanda> Listar(long usuarioId)
        {
            return _contextoEF.PontosDemanda.Where(d => d.GrupoDeIntegrantes.Integrantes.Any(i => i.Usuario.Id == usuarioId)).ToList();
        }

        public PontoDemanda Obter(long id, long usuarioId)
        {
            var pontoDemanda = _contextoEF.PontosDemanda.SingleOrDefault(d => d.GrupoDeIntegrantes.Integrantes.Any(i => i.Usuario.Id == usuarioId) && d.Id == id);
            return pontoDemanda;
        }

        public void DefinirFrequenciaDeConsumo(long pontoDemandaId, long usuarioId, int frequencia)
        {
            throw new NotImplementedException("Use o metodo da classe PontoDemandaADO do pacote LM.Core.Repository");
        }

        public PontoDemanda Criar(PontoDemanda pontoDemanda)
        {
            _contextoEF.Entry(pontoDemanda.GrupoDeIntegrantes).State = EntityState.Modified;
            _contextoEF.Entry(pontoDemanda.Endereco.Cidade).State = EntityState.Unchanged;
            _contextoEF.PontosDemanda.Add(pontoDemanda);
            _contextoEF.SaveChanges();
            return pontoDemanda;
        }
    }
}
