namespace LM.Core.RepositorioEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddEhSubstitutoToListaItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TB_LISTA_PRODUTO_ITEM", "FL_SUBSTITUTO", c => c.Boolean(nullable: false, defaultValue: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TB_LISTA_PRODUTO_ITEM", "FL_SUBSTITUTO");
        }
    }
}
