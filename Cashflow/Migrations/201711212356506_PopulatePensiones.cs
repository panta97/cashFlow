namespace Cashflow.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PopulatePensiones : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO Pensiones (Id, Nombre, Aporte, Seguro, Comision) VALUES (1, 'ONP', 0.13, 0.0167, 0.0143)");
            Sql("INSERT INTO Pensiones (Id, Nombre, Aporte, Seguro, Comision) VALUES (2, 'AFP Habitat', 0.1, 0.0136, 0.0038)");
            Sql("INSERT INTO Pensiones (Id, Nombre, Aporte, Seguro, Comision) VALUES (3, 'AFP Integra', 0.1, 0.0136, 0.009)");
            Sql("INSERT INTO Pensiones (Id, Nombre, Aporte, Seguro, Comision) VALUES (4, 'AFP Prima', 0.1, 0.0136, 0.0018)");
            Sql("INSERT INTO Pensiones (Id, Nombre, Aporte, Seguro, Comision) VALUES (5, 'AFP Profuturo', 0.1, 0.0136, 0.0107)");


        }

        public override void Down()
        {
        }
    }
}
