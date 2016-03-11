using PG.Domain;
using PG.Infra.Mappings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PG.Infra.DataContents
{
    
    public class PGDataContents: DbContext
    {
        
        public PGDataContents(): base("PGConnection")
        {
            Database.SetInitializer<PGDataContents>(null);
            //AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Directory.GetCurrentDirectory());
            Configuration.LazyLoadingEnabled = false;
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new BancoMap());
            modelBuilder.Configurations.Add(new ContaContabilMap());
            modelBuilder.Configurations.Add(new EnderecoMap());
            modelBuilder.Configurations.Add(new EquipeMap());
            modelBuilder.Configurations.Add(new FaturamentoMap());
            modelBuilder.Configurations.Add(new FuncionarioMap());
            modelBuilder.Configurations.Add(new OrdemServicoMap());
            modelBuilder.Configurations.Add(new PCMap());
            modelBuilder.Configurations.Add(new TipoFixacaoMap());
            modelBuilder.Configurations.Add(new TipoInstalacaoMap());
            modelBuilder.Configurations.Add(new TipoMaquinaMap());

            base.OnModelCreating(modelBuilder);
        }


        public DbSet<Banco> Bancos { get; set; }
        public DbSet<ContaContabil> ContasContabil { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Equipe> Equipes { get; set; }
        public DbSet<Faturamento> Faturamentos { get; set; }
        public DbSet<Funcionario> Funcionarios { get; set; }
        public DbSet<OrdemServico> OrdensServico { get; set; }
        public DbSet<PC> PCs { get; set; }
        public DbSet<TipoFixacao> TpFixacao { get; set; }
        public DbSet<TipoInstalacao> TpInstalacao { get; set; }
        public DbSet<TipoMaquina> TpMaquina { get; set; }

    }
}
