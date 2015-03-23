using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LM.Core.Domain;

namespace LM.Core.RepositorioEF
{
    public class PontoDemandaEF
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
    }
}
