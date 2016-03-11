using DevStore.Domain;
using DevStore.Infra.Mapping;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevStore.Infra.DataContets
{
    public class DevStoreDataContents: DbContext 
    {
        public DevStoreDataContents(): base("DevStoreConnectionString")
        {
            // Sempre vai dropar e criar um novo banco de dados baseados nas classes do dominio
            Database.SetInitializer<DevStoreDataContents>(new DevStoreDataContextInitializer());
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CategoryMap());
            modelBuilder.Configurations.Add(new ProductMap());
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Produto> Produts { get; set; }
        public DbSet<Category> Categories { get; set; }

    }


    public class DevStoreDataContextInitializer : DropCreateDatabaseIfModelChanges<DevStoreDataContents>
    {
        protected override void Seed(DevStoreDataContents context)
        {
            context.Categories.Add(new Category { Id = 1, Title = "Categoria 1" });
            context.Categories.Add(new Category { Id = 2, Title = "Categoria 2" });
            context.Categories.Add(new Category { Id = 3, Title = "Categoria 3" });
            context.SaveChanges();

            context.Produts.Add(new Produto { Id = 1, Title = "Produto 1", CategoryId = 1 , IsActive= true});
            context.Produts.Add(new Produto { Id = 2, Title = "Produto 2", CategoryId = 2 , IsActive = true});
            context.Produts.Add(new Produto { Id = 3, Title = "Produto 3", CategoryId = 3 , IsActive = true});
            context.SaveChanges();

            base.Seed(context);
        }
    }
}
