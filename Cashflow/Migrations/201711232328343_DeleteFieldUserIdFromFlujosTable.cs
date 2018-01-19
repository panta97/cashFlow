namespace Cashflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteFieldUserIdFromFlujosTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Flujos", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Flujos", new[] { "ApplicationUserId" });
            DropColumn("dbo.Flujos", "ApplicationUserId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Flujos", "ApplicationUserId", c => c.String(maxLength: 128));
            CreateIndex("dbo.Flujos", "ApplicationUserId");
            AddForeignKey("dbo.Flujos", "ApplicationUserId", "dbo.AspNetUsers", "Id");
        }
    }
}
