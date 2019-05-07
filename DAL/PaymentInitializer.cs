using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using EcheancierDotNet.Models;

namespace EcheancierDotNet.DAL
{
    public class PaymentInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<PaymentContext>
    {
        protected override void Seed(PaymentContext context)
        {
            var suppliers = new List<Supplier>
            {
            new Supplier{ID=0,SAPAccountNumber=401002474,SAPMainAccountNumber=401000000,Name="ANODIGRAV", Bank="SOCIETE GENERALE", BankCode=30003, Guichet=3620, AccountNumber=20013510, BankKey=14, BankAddress="", BIC="SOGEFRPP", IBAN="FR7630003036200002001351014", BankCountry="FR", PaymentDelay=30},
            new Supplier{ID=1,SAPAccountNumber=401002474,SAPMainAccountNumber=401000000,Name="Plastifrance", Bank="BPACA", BankCode=30003, Guichet=3620, AccountNumber=20013510, BankKey=14, BankAddress="", BIC="SOGEFRPP", IBAN="FR7630003036200002001351014", BankCountry="FR", PaymentDelay=30},
            new Supplier{ID=2,SAPAccountNumber=401002474,SAPMainAccountNumber=401000000,Name="BOLOFF", Bank="BNP", BankCode=30003, Guichet=3620, AccountNumber=20013510, BankKey=14, BankAddress="", BIC="SOGEFRPP", IBAN="FR7630003036200002001351014", BankCountry="FR", PaymentDelay=30},
            new Supplier{ID=3,SAPAccountNumber=401002474,SAPMainAccountNumber=401000000,Name="AEDS", Bank="BNP", BankCode=30003, Guichet=3620, AccountNumber=20013510, BankKey=14, BankAddress="", BIC="SOGEFRPP", IBAN="FR7630003036200002001351014", BankCountry="FR", PaymentDelay=30}
            };

            suppliers.ForEach(s => context.Suppliers.Add(s));
            context.SaveChanges();


            var invoices = new List<Invoice>
            {
            new Invoice{SupplierID=0,Currency="EUR",DocumentNumber="21531001",DocumentReference="2019FACT00054",DocumentHeader="X21531",DocumentDate=DateTime.Parse("15-01-2019"),DueDate=DateTime.Parse("15-03-2019"),RawAmount=0,VAT=0,DueAmount= 21958, ToBePaid=true, Paid=false, ProForma=false, Comment=""},
            new Invoice{SupplierID=1,Currency="EUR",DocumentNumber="21535002",DocumentReference="2019FACT00055",DocumentHeader="X21531",DocumentDate=DateTime.Parse("15-01-2019"),DueDate=DateTime.Parse("20-03-2019"),RawAmount=0,VAT=0,DueAmount= 5000, ToBePaid=true, Paid=false, ProForma=false, Comment=""},
            new Invoice{SupplierID=2,Currency="EUR",DocumentNumber="21531003",DocumentReference="2019FACT00056",DocumentHeader="X21531",DocumentDate=DateTime.Parse("15-01-2019"),DueDate=DateTime.Parse("15-04-2019"),RawAmount=0,VAT=0,DueAmount= 41000, ToBePaid=true, Paid=false, ProForma=false, Comment=""},
            new Invoice{SupplierID=3,Currency="EUR",DocumentNumber="21211004",DocumentReference="2019FACT00057",DocumentHeader="X21531",DocumentDate=DateTime.Parse("15-01-2019"),DueDate=DateTime.Parse("15-05-2019"),RawAmount=0,VAT=0,DueAmount= 958, ToBePaid=false, Paid=false, ProForma=false, Comment=""},
            new Invoice{SupplierID=0,Currency="EUR",DocumentNumber="21531005",DocumentReference="2019FACT00058",DocumentHeader="X21531",DocumentDate=DateTime.Parse("15-01-2019"),DueDate=DateTime.Parse("20-05-2019"),RawAmount=0,VAT=0,DueAmount= 32514, ToBePaid=false, Paid=false, ProForma=false, Comment=""},
            new Invoice{SupplierID=1,Currency="EUR",DocumentNumber="21531006",DocumentReference="2019FACT00059",DocumentHeader="X21531",DocumentDate=DateTime.Parse("15-01-2019"),DueDate=DateTime.Parse("25-05-2019"),RawAmount=0,VAT=0,DueAmount= 3214, ToBePaid=false, Paid=false, ProForma=false, Comment=""},
            new Invoice{SupplierID=2,Currency="EUR",DocumentNumber="21531007",DocumentReference="2019FACT00060",DocumentHeader="X21531",DocumentDate=DateTime.Parse("15-01-2019"),DueDate=DateTime.Parse("28-05-2019"),RawAmount=0,VAT=0,DueAmount= 10004, ToBePaid=false, Paid=false, ProForma=false, Comment=""},
            new Invoice{SupplierID=3,Currency="EUR",DocumentNumber="21531008",DocumentReference="2019FACT00061",DocumentHeader="X21531",DocumentDate=DateTime.Parse("15-01-2019"),DueDate=DateTime.Parse("12-05-2019"),RawAmount=0,VAT=0,DueAmount= 50554, ToBePaid=false, Paid=false, ProForma=false, Comment=""},
            new Invoice{SupplierID=3,Currency="USD",DocumentNumber="21531009",DocumentReference="2019FACT00062",DocumentHeader="X21531",DocumentDate=DateTime.Parse("15-01-2019"),DueDate=DateTime.Parse("21-05-2019"),RawAmount=0,VAT=0,DueAmount= 1254, ToBePaid=false, Paid=false, ProForma=false, Comment=""},
            new Invoice{SupplierID=2,Currency="USD",DocumentNumber="21531010",DocumentReference="2019FACT00063",DocumentHeader="X21531",DocumentDate=DateTime.Parse("15-01-2019"),DueDate=DateTime.Parse("15-06-2019"),RawAmount=0,VAT=0,DueAmount= 250, ToBePaid=false, Paid=false, ProForma=false, Comment=""},
            new Invoice{SupplierID=2,Currency="USD",DocumentNumber="21531011",DocumentReference="2019FACT00064",DocumentHeader="X21531",DocumentDate=DateTime.Parse("15-01-2019"),DueDate=DateTime.Parse("06-05-2019"),RawAmount=0,VAT=0,DueAmount= 2148, ToBePaid=false, Paid=false, ProForma=false, Comment=""},
            new Invoice{SupplierID=1,Currency="EUR",DocumentNumber="21531012",DocumentReference="2019FACT00065",DocumentHeader="X21531",DocumentDate=DateTime.Parse("15-01-2019"),DueDate=DateTime.Parse("04-05-2019"),RawAmount=0,VAT=0,DueAmount= 8452, ToBePaid=false, Paid=false, ProForma=false, Comment=""},
            new Invoice{SupplierID=3,Currency="ESD",DocumentNumber="21531013",DocumentReference="2019FACT00066",DocumentHeader="X21531",DocumentDate=DateTime.Parse("15-01-2019"),DueDate=DateTime.Parse("30-05-2019"),RawAmount=0,VAT=0,DueAmount= 2658, ToBePaid=false, Paid=false, ProForma=false, Comment=""},
            new Invoice{SupplierID=2,Currency="USD",DocumentNumber="21531014",DocumentReference="2019FACT00067",DocumentHeader="X21531",DocumentDate=DateTime.Parse("15-01-2019"),DueDate=DateTime.Parse("05-06-2019"),RawAmount=0,VAT=0,DueAmount= 4999, ToBePaid=false, Paid=false, ProForma=false, Comment=""},
            new Invoice{SupplierID=0,Currency="EUR",DocumentNumber="21531015",DocumentReference="2019FACT00068",DocumentHeader="X21531",DocumentDate=DateTime.Parse("15-01-2019"),DueDate=DateTime.Parse("02-07-2019"),RawAmount=0,VAT=0,DueAmount= 102444, ToBePaid=false, Paid=false, ProForma=false, Comment=""}
            };
            invoices.ForEach(s => context.Invoices.Add(s));
            context.SaveChanges();

        }

        }


}