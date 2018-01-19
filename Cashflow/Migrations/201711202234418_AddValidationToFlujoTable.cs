namespace Cashflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddValidationToFlujoTable : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Flujos", "Nombre", c => c.String(nullable: false, maxLength: 255));
            AlterColumn("dbo.Flujos", "FechaFin", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Flujos", "FechaFin", c => c.DateTime());
            AlterColumn("dbo.Flujos", "Nombre", c => c.String(maxLength: 255));
        }
    }
}
