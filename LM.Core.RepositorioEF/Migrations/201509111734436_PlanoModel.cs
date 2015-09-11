namespace LM.Core.RepositorioEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PlanoModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TB_PLANO",
                c => new
                    {
                        ID_PLANO = c.Int(nullable: false, identity: true),
                        NM_PLANO = c.String(),
                        VL_PERIODO = c.Int(nullable: false),
                        VL_VALOR = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TX_CHAMADA = c.String(),
                        DT_INC = c.DateTime(nullable: false),
                        DT_ALT = c.DateTime(nullable: false),
                        FL_ATIVO = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID_PLANO);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TB_PLANO");
        }
    }
}
