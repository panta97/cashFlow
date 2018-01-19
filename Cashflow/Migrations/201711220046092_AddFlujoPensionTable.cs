namespace Cashflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFlujoPensionTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FlujoPensiones",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FlujoId = c.Int(nullable: false),
                        PensionId = c.Byte(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Flujos", t => t.FlujoId, cascadeDelete: true)
                .ForeignKey("dbo.Pensiones", t => t.PensionId, cascadeDelete: true)
                .Index(t => t.FlujoId)
                .Index(t => t.PensionId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FlujoPensiones", "PensionId", "dbo.Pensiones");
            DropForeignKey("dbo.FlujoPensiones", "FlujoId", "dbo.Flujos");
            DropIndex("dbo.FlujoPensiones", new[] { "PensionId" });
            DropIndex("dbo.FlujoPensiones", new[] { "FlujoId" });
            DropTable("dbo.FlujoPensiones");
        }
    }
}
