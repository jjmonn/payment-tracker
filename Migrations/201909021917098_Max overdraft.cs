namespace EcheancierDotNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Maxoverdraft : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BankAccount", "MaxOverdraft", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BankAccount", "MaxOverdraft");
        }
    }
}
