namespace EcheancierDotNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BankAccount",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        BankCode = c.Int(nullable: false),
                        AgencyCode = c.Int(nullable: false),
                        AccountNumber = c.Int(nullable: false),
                        RIBKey = c.Int(nullable: false),
                        BIC = c.String(),
                        IBAN = c.String(),
                        Currency = c.String(),
                        BankAddress = c.String(),
                        BankCountry = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Flow",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SupplierID = c.Int(nullable: false),
                        Currency = c.String(),
                        DocumentReference = c.String(),
                        DocumentHeader = c.String(),
                        DueDate = c.DateTime(nullable: false),
                        RawAmount = c.Double(nullable: false),
                        VAT = c.Double(nullable: false),
                        DueAmount = c.Double(nullable: false),
                        Comment = c.String(),
                        BankID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Invoice",
                c => new
                    {
                        InvoiceID = c.Int(nullable: false, identity: true),
                        SupplierID = c.Int(nullable: false),
                        Currency = c.String(),
                        DocumentNumber = c.String(),
                        DocumentReference = c.String(),
                        DocumentHeader = c.String(),
                        DocumentDate = c.DateTime(nullable: false),
                        DueDate = c.DateTime(nullable: false),
                        GoodsReceptionDate = c.DateTime(),
                        RawAmount = c.Double(nullable: false),
                        VAT = c.Double(nullable: false),
                        DueAmount = c.Double(nullable: false),
                        ToBePaid = c.Boolean(nullable: false),
                        Paid = c.Boolean(nullable: false),
                        ProForma = c.Boolean(nullable: false),
                        Comment = c.String(),
                        BankID = c.Int(nullable: false),
                        PaymentDate = c.DateTime(),
                        UploadID = c.Int(nullable: false),
                        PaymentMethod = c.Int(nullable: false),
                        BankAccount_ID = c.Int(),
                    })
                .PrimaryKey(t => t.InvoiceID)
                .ForeignKey("dbo.BankAccount", t => t.BankAccount_ID)
                .ForeignKey("dbo.Supplier", t => t.SupplierID, cascadeDelete: true)
                .ForeignKey("dbo.Upload", t => t.UploadID, cascadeDelete: true)
                .Index(t => t.SupplierID)
                .Index(t => t.UploadID)
                .Index(t => t.BankAccount_ID);
            
            CreateTable(
                "dbo.Supplier",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        SAPAccountNumber = c.Int(nullable: false),
                        SAPMainAccountNumber = c.Int(nullable: false),
                        Name = c.String(),
                        Bank = c.String(),
                        BankCode = c.Int(nullable: false),
                        Guichet = c.Int(nullable: false),
                        AccountNumber = c.Int(nullable: false),
                        BankKey = c.Int(nullable: false),
                        BankAddress = c.String(),
                        BIC = c.String(),
                        IBAN = c.String(),
                        BankCountry = c.String(),
                        Currency = c.String(),
                        IsProForma = c.Boolean(nullable: false),
                        IsInterco = c.Boolean(nullable: false),
                        PaymentDelay = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Upload",
                c => new
                    {
                        UploadID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        UploadDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.UploadID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Invoice", "UploadID", "dbo.Upload");
            DropForeignKey("dbo.Invoice", "SupplierID", "dbo.Supplier");
            DropForeignKey("dbo.Invoice", "BankAccount_ID", "dbo.BankAccount");
            DropIndex("dbo.Invoice", new[] { "BankAccount_ID" });
            DropIndex("dbo.Invoice", new[] { "UploadID" });
            DropIndex("dbo.Invoice", new[] { "SupplierID" });
            DropTable("dbo.Upload");
            DropTable("dbo.Supplier");
            DropTable("dbo.Invoice");
            DropTable("dbo.Flow");
            DropTable("dbo.BankAccount");
        }
    }
}
