using System.Data.Entity;
using LM.Core.Domain;

namespace LM.Core.RepositorioEF
{
    public class ContextoCorreiosEF : DbContext
    {
        public DbSet<EnderecoCorreios> EnderecosCorreios { get; set; }

        public ContextoCorreiosEF() : base("Correios")
        {
            Database.SetInitializer<ContextoCorreiosEF>(null);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EnderecoCorreios>().HasKey(e => e.Cep);
        }
    }
}

