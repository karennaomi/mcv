using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class PlanoEF : IRepositorioPlano
    {
        private readonly ContextoEF _contexto;
        public PlanoEF()
        {
            _contexto = new ContextoEF();
        }

        public IList<Plano> Listar()
        {
            return _contexto.Planos.AsNoTracking().Where(p => p.Ativo).ToList();
        }
    }
}
