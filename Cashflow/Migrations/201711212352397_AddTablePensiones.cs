namespace Cashflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddTablePensiones : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Pensiones",
                c => new
                    {
                        Id = c.Byte(nullable: false),
                        Aporte = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Seguro = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Comision = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Pensiones");
        }
    }
}
