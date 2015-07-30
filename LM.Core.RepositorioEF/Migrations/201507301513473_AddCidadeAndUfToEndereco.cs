namespace LM.Core.RepositorioEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddCidadeAndUfToEndereco : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TB_ENDERECO", "NM_CIDADE", c => c.String(maxLength:50));
            AddColumn("dbo.TB_ENDERECO", "NM_UF", c => c.String(maxLength:2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TB_ENDERECO", "NM_UF");
            DropColumn("dbo.TB_ENDERECO", "NM_CIDADE");
        }
    }
}
