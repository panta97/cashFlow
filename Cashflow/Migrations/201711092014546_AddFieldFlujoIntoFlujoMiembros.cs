namespace Cashflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFieldFlujoIntoFlujoMiembros : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.FlujoMiembros", "FlujoId", c => c.Int(nullable: false));
            CreateIndex("dbo.FlujoMiembros", "FlujoId");
            AddForeignKey("dbo.FlujoMiembros", "FlujoId", "dbo.Flujos", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.FlujoMiembros", "FlujoId", "dbo.Flujos");
            DropIndex("dbo.FlujoMiembros", new[] { "FlujoId" });
            DropColumn("dbo.FlujoMiembros", "FlujoId");
        }
    }
}
