namespace EcheancierDotNet.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TurboSuppliersTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TurboSupplier",
                c => new
                    {
                        TurboSupplierID = c.Int(nullable: false, identity: true),
                        PaymentArea = c.Int(nullable: false),
                        TYPE = c.String(),
                        BENEF_CODE = c.Int(nullable: false),
                        BENEF_RS = c.String(),
                        SIREN = c.String(),
                        NOM = c.String(),
                        BENEF_AD1 = c.String(),
                        BENEF_CP = c.String(),
                        BENEF_VILLE = c.String(),
                        BENEF_MAIL = c.String(),
                        BENEF_CODEPAYS = c.String(),
                        DOB_IBAN = c.String(),
                        BQE_BIC = c.String(),
                        BQE_NOM = c.String(),
                        BQE_CODEPAYS = c.String(),
                        GROUPE = c.String(),
                        MONTANT = c.Double(nullable: false),
                        DEVISE = c.String(),
                        SOCIETE_BENEF = c.String(),
                        DOB_TYPE = c.String(),
                    })
                .PrimaryKey(t => t.TurboSupplierID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.TurboSupplier");
        }
    }
}
