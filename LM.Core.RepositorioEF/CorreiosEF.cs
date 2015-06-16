using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Linq;

namespace LM.Core.RepositorioEF
{
    public class CorreiosEF : IRepositorioCorreios
    {
        private readonly ContextoCorreiosEF _contexto;

        public CorreiosEF()
        {
            _contexto = new ContextoCorreiosEF();
        }

        public EnderecoCorreios BuscarPorCep(string cep)
        {
            return _contexto.EnderecosCorreios.FirstOrDefault(c => c.Cep == cep.Replace("-", ""));
        }
    }
}
