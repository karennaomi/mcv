using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contador.Domain;
using Contador.Infra;
using Contador.Infra.Mappings;

namespace Contador.Infra.DataContents
{
    public class ContadorDataContents : DbContext
    {
        public ContadorDataContents(): base("ContadorConnectionString")
        {
            // Sempre vai dropar e criar um novo banco de dados baseados nas classes do dominio
            //Database.SetInitializer<ContadorDataContents>(new ContadorDataContextInitializer());
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            //modelBuilder.Configurations.Add(new EmpresaMap());
            //modelBuilder.Configurations.Add(new NFMap());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Empresa> Empresas { get; set; }

    }

    public class ContadorDataContextInitializer : DropCreateDatabaseIfModelChanges<ContadorDataContents>
    {
        protected override void Seed(ContadorDataContents context)
        {

            context.Users.Add(new User { Id = 1, Nome = "Karen", CPF = "2222222" });
            context.SaveChanges();
            base.Seed(context);
        }

    }
}
