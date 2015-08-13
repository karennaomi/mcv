namespace LM.Core.RepositorioEF.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    public sealed class Configuration : DbMigrationsConfiguration<LM.Core.RepositorioEF.ContextoEF>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(LM.Core.RepositorioEF.ContextoEF context)
        {
            context.MotivosSubstituicao.Add(new Domain.MotivoSubstituicao { Motivo = "Não encontrei", Ativo = true });
            context.MotivosSubstituicao.Add(new Domain.MotivoSubstituicao { Motivo = "Preço", Ativo = true });

            context.SaveChanges();
        }
    }
}
