using System.Data.Entity;
using System.Data.Entity.Migrations;
using LM.Core.RepositorioEF;
using NUnit.Framework;
using LM.Core.Tests.Migrations;

namespace LM.Core.Tests
{
    [SetUpFixture]
    public class SetupTests
    {
        [SetUp]
        public void RunBeforeAnyTests()
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ContextoEF, Configuration>());
            var contexto = new ContextoEF();
            Database.Delete(contexto.Database.Connection);            
            var migrator = new DbMigrator(new Configuration());
            migrator.Update();
        }

        [TearDown]
        public void RunAfterAnyTests()
        {
        }
    }
}
