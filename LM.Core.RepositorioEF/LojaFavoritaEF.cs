using System.Data.Entity;
using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class LojaFavoritaEF : IRepositorioLojaFavorita
    {
        private readonly ContextoEF _contexto;
        public LojaFavoritaEF(ContextoEF contexto)
        {
            _contexto = contexto;
        }

        public Loja VerificarLojaExistente(Loja loja)
        {
            var lojaExistente = _contexto.Lojas.SingleOrDefault(l => l.Idlocalizador == loja.Idlocalizador);
            return lojaExistente ?? loja;
        }
    }
}
