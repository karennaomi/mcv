using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Collections.Generic;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class UfEF : IRepositorioUf
    {
        private readonly ContextoEF _contexto;
        public UfEF()
        {
            _contexto = new ContextoEF();
        }

        public IList<Uf> Listar()
        {
            return _contexto.Ufs.AsNoTracking().ToList();
        }
    }
}
