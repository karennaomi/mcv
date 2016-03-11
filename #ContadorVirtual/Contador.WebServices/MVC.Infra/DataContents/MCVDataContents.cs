using MCV.Domain;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCV.Infra.Mappings;

namespace MCV.Infra.DataContents
{
    public class MCVDataContents: DbContext
    {
        public MCVDataContents() : base("MCVConnection")
        {
            Database.SetInitializer<MCVDataContents>(null);
            //AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new EmpresasMap());
            modelBuilder.Configurations.Add(new EnderecoMap());


            base.OnModelCreating(modelBuilder);
        }


        public DbSet<User> Users { get; set; }
        public DbSet<Empresas> Empresas { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
    }

    

}
