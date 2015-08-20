namespace LM.Core.RepositorioEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateProdutoModelValidation : DbMigration
    {
        public override void Up()
        {
            //desabilitando alterações pq as colunas estao com indices de full text search
            //AlterColumn("dbo.TB_PRODUTO", "CD_PRODUTO_EAN", c => c.String(maxLength: 13));
            //AlterColumn("dbo.TB_PRODUTO_MASTER_DATA", "NM_PRODUTO_COMPLETO", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            //AlterColumn("dbo.TB_PRODUTO_MASTER_DATA", "NM_PRODUTO_COMPLETO", c => c.String());
            //AlterColumn("dbo.TB_PRODUTO", "CD_PRODUTO_EAN", c => c.String(nullable: false, maxLength: 13));
        }
    }
}
