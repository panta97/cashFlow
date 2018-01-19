namespace Cashflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddValidationToMiembroTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Miembros", "Nombre", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Miembros", "Nombre", c => c.String());
        }
    }
}
