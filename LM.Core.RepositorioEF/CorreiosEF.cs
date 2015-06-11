using LM.Core.Domain;
using LM.Core.Domain.Repositorio;
using System.Data.SqlClient;
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
            return _contexto.Database.SqlQuery<EnderecoCorreios>("SP_BUSCA_CEP @CEP", new SqlParameter("@CEP", cep)).FirstOrDefault();
        }
    }
}
