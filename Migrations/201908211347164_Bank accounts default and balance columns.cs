namespace EcheancierDotNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Bankaccountsdefaultandbalancecolumns : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.BankAccount", "Balance", c => c.Double(nullable: false));
            AddColumn("dbo.BankAccount", "DefaultBank", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.BankAccount", "DefaultBank");
            DropColumn("dbo.BankAccount", "Balance");
        }
    }
}
