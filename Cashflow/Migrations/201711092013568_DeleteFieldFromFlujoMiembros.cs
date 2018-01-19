namespace Cashflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteFieldFromFlujoMiembros : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.FlujoMiembros", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.FlujoMiembros", new[] { "ApplicationUserId" });
            DropColumn("dbo.FlujoMiembros", "ApplicationUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.FlujoMiembros", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.FlujoMiembros", "ApplicationUserId");
            AddForeignKey("dbo.FlujoMiembros", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
