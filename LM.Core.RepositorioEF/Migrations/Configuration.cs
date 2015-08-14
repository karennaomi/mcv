using System.Data.Entity.Migrations;

namespace LM.Core.RepositorioEF.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<LM.Core.RepositorioEF.ContextoEF>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LM.Core.RepositorioEF.ContextoEF context)
        {
        }
    }
}
