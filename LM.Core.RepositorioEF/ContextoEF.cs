using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using LM.Core.Domain;
using LM.Core.RepositorioEF.MappingConfiguration;

namespace LM.Core.RepositorioEF
{
    public class ContextoEF : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<PontoDemanda> PontosDemanda { get; set; }
        public DbSet<Integrante> Integrantes { get; set; }
        public DbSet<Persona> Personas { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Lista> Listas { get; set; }
        public DbSet<PedidoItem> PedidoItens { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Cidade> Cidades { get; set; }
        public DbSet<CompraAtiva> ComprasAtivas { get; set; }
        public DbSet<TemplateMensagem> TemplatesMensagens { get; set; }
        public DbSet<Loja> Lojas { get; set; }
        public DbSet<RecuperarSenha> RecuperarSenhas { get; set; }
        public DbSet<FilaItem> FilaItens { get; set; }
        public DbSet<LegacyToken> LegacyTokens { get; set; }

        public ContextoEF() : base("SOL")
        {
            Database.SetInitializer<ContextoEF>(null);
            DbInterception.Add(new FtsInterceptor());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new UsuarioConfig());
            modelBuilder.Configurations.Add(new StatusUsuarioPontoDemandaConfig());
            modelBuilder.Configurations.Add(new PontoDemandaConfig());
            modelBuilder.Configurations.Add(new GrupoDeIntegrantesConfig());
            modelBuilder.Configurations.Add(new IntegranteConfig());
            modelBuilder.Configurations.Add(new PersonaConfig());
            modelBuilder.Configurations.Add(new EnderecoConfig());
            modelBuilder.Configurations.Add(new CidadeConfig());
            modelBuilder.Configurations.Add(new UfConfig());
            modelBuilder.Configurations.Add(new CategoriaConfig());
            modelBuilder.Configurations.Add(new ProdutoConfig());
            modelBuilder.Configurations.Add(new ProdutoInfoConfig());
            modelBuilder.Configurations.Add(new ImagemConfig());
            modelBuilder.Configurations.Add(new ListaConfig());
            modelBuilder.Configurations.Add(new ListaItemConfig());
            modelBuilder.Configurations.Add(new PedidoItemConfig());
            modelBuilder.Configurations.Add(new PeriodoConfig());
            modelBuilder.Configurations.Add(new CompraConfig());
            modelBuilder.Configurations.Add(new CompraItemConfig());
            modelBuilder.Configurations.Add(new ListaCompraItemConfig());
            modelBuilder.Configurations.Add(new PedidoCompraItemConfig());
            modelBuilder.Configurations.Add(new CompraItemSubstitutoConfig());
            modelBuilder.Configurations.Add(new CompraAtivaConfig());
            modelBuilder.Configurations.Add(new TemplateMensagemConfig());
            modelBuilder.Configurations.Add(new TemplateMensagemPushConfig());
            modelBuilder.Configurations.Add(new TemplateMensagemEmailConfig());
            modelBuilder.Configurations.Add(new LojaConfig());
            modelBuilder.Configurations.Add(new LojaInfoConfig());
            modelBuilder.Configurations.Add(new RecuperarSenhaConfig());
            modelBuilder.Configurations.Add(new FilaItemConfig());
            modelBuilder.Configurations.Add(new FilaMensagemConfig());
            modelBuilder.Configurations.Add(new FilaMensagemEmailConfig());
            modelBuilder.Configurations.Add(new FilaMensagemSmsConfig());
            modelBuilder.Configurations.Add(new LegacyTokenConfig());
        }
    }
}

