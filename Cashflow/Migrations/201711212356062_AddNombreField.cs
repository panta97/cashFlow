namespace Cashflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddNombreField : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pensiones", "Nombre", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pensiones", "Nombre");
        }
    }
}
