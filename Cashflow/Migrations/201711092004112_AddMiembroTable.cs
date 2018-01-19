namespace Cashflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMiembroTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Miembros",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Edad = c.Byte(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Miembros", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Miembros", new[] { "ApplicationUserId" });
            DropTable("dbo.Miembros");
        }
    }
}
