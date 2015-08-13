namespace LM.Core.RepositorioEF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class MotivoSubstituicao : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TB_Motivo_Substituicao",
                c => new
                    {
                        ID_MOTIVO = c.Int(nullable: false, identity: true),
                        NM_MOTIVO = c.String(),
                        FL_ATIVO = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ID_MOTIVO);
            
            AddColumn("dbo.TB_Compra_Item_Substituto", "ID_MOTIVO", c => c.Int(nullable: false));
            CreateIndex("dbo.TB_Compra_Item_Substituto", "ID_MOTIVO");
            AddForeignKey("dbo.TB_Compra_Item_Substituto", "ID_MOTIVO", "dbo.TB_Motivo_Substituicao", "ID_MOTIVO", cascadeDelete: true);
            DropColumn("dbo.TB_Compra_Item_Substituto", "TX_MOTIVO");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TB_Compra_Item_Substituto", "TX_MOTIVO", c => c.String());
            DropForeignKey("dbo.TB_Compra_Item_Substituto", "ID_MOTIVO", "dbo.TB_Motivo_Substituicao");
            DropIndex("dbo.TB_Compra_Item_Substituto", new[] { "ID_MOTIVO" });
            DropColumn("dbo.TB_Compra_Item_Substituto", "ID_MOTIVO");
            DropTable("dbo.TB_Motivo_Substituicao");
        }
    }
}
