namespace LM.Core.RepositorioEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AssinaturaModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TB_ASSINATURA",
                c => new
                    {
                        ID_ASSINATURA = c.Int(nullable: false, identity: true),
                        DT_INC = c.DateTime(nullable: false),
                        DT_ALT = c.DateTime(nullable: false),
                        FL_STATUS = c.Int(nullable: false),
                        ID_PLANO = c.Int(nullable: false),
                        ID_USUARIO = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.ID_ASSINATURA)
                .ForeignKey("dbo.TB_PLANO", t => t.ID_PLANO, cascadeDelete: true)
                .ForeignKey("dbo.TB_USUARIO", t => t.ID_USUARIO, cascadeDelete: true)
                .Index(t => t.ID_PLANO)
                .Index(t => t.ID_USUARIO);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TB_ASSINATURA", "ID_USUARIO", "dbo.TB_USUARIO");
            DropForeignKey("dbo.TB_ASSINATURA", "ID_PLANO", "dbo.TB_PLANO");
            DropIndex("dbo.TB_ASSINATURA", new[] { "ID_USUARIO" });
            DropIndex("dbo.TB_ASSINATURA", new[] { "ID_PLANO" });
            DropTable("dbo.TB_ASSINATURA");
        }
    }
}
