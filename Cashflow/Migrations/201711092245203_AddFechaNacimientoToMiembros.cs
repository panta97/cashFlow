namespace Cashflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFechaNacimientoToMiembros : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Miembros", "FechaNacimiento", c => c.DateTime(nullable: false));
            DropColumn("dbo.Miembros", "Edad");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Miembros", "Edad", c => c.Byte(nullable: false));
            DropColumn("dbo.Miembros", "FechaNacimiento");
        }
    }
}
