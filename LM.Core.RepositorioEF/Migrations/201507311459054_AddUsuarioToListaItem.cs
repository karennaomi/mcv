namespace LM.Core.RepositorioEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUsuarioToListaItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TB_LISTA_PRODUTO_ITEM", "ID_USUARIO", c => c.Long());
            CreateIndex("dbo.TB_LISTA_PRODUTO_ITEM", "ID_USUARIO");
            AddForeignKey("dbo.TB_LISTA_PRODUTO_ITEM", "ID_USUARIO", "dbo.TB_USUARIO", "ID_USUARIO");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TB_LISTA_PRODUTO_ITEM", "ID_USUARIO", "dbo.TB_USUARIO");
            DropIndex("dbo.TB_LISTA_PRODUTO_ITEM", new[] { "ID_USUARIO" });
            DropColumn("dbo.TB_LISTA_PRODUTO_ITEM", "ID_USUARIO");
        }
    }
}
