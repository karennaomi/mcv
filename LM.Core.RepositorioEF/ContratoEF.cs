using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class ContratoEF : IRepositorioContrato
    {
        private readonly ContextoEF _contexto;
        public ContratoEF()
        {
            _contexto = new ContextoEF();
        }

        public Contrato ObterAtivo()
        {
            return _contexto.Contratos.AsNoTracking().FirstOrDefault(c => c.Ativo);
        }
    }
}
