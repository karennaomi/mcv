using System.Data.Entity;

namespace LM.Core.RepositorioEF
{
    public class ContextoCorreiosEF : DbContext
    {

        public ContextoCorreiosEF() : base("Correios")
        {
            Database.SetInitializer<ContextoCorreiosEF>(null);
        }
    }
}

