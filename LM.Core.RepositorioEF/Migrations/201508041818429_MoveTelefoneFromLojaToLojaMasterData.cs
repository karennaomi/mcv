namespace LM.Core.RepositorioEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MoveTelefoneFromLojaToLojaMasterData : DbMigration
    {
        public override void Up()
        {
            //Comentando pois a coluna ja existe no banco
            //AddColumn("dbo.TB_Loja_Master_Data", "TX_TELEFONE_LOJA", c => c.String());
        }
        
        public override void Down()
        {
            //DropColumn("dbo.TB_Loja_Master_Data", "TX_TELEFONE_LOJA");
        }
    }
}
