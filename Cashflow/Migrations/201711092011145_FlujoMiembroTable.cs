namespace Cashflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FlujoMiembroTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FlujoMiembros",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ApplicationUserId = c.String(maxLength: 128),
                        MiembroId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.Miembros", t => t.MiembroId, cascadeDelete: true)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.MiembroId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FlujoMiembros", "MiembroId", "dbo.Miembros");
            DropForeignKey("dbo.FlujoMiembros", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.FlujoMiembros", new[] { "MiembroId" });
            DropIndex("dbo.FlujoMiembros", new[] { "ApplicationUserId" });
            DropTable("dbo.FlujoMiembros");
        }
    }
}
