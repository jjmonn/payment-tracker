using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcheancierDotNet.Models
{
    
    public enum PaymentMehtod
    {
        BankWire,
        Cash,
        Check
    }

    public enum Currency
    {
        EUR,
        USD,
        GBP
    }

    public class Invoice
    {
        public int InvoiceID { get; set; }
        public int SupplierID { get; set; }
        public string Currency { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentReference { get; set; }
        public string DocumentHeader { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime DueDate { get; set; }
        public Nullable<DateTime>GoodsReceptionDate { get; set; }
        public double RawAmount { get; set; }
        public double VAT { get; set; }
        public double DueAmount { get; set; }
        public bool ToBePaid { get; set; }
        public bool Paid { get; set; }
        public bool ProForma { get; set; }
        public string Comment { get; set; }
        public int BankID { get; set; }
        public Nullable<DateTime>PaymentDate { get; set; }
        public int UploadID { get; set; }
        public int PaymentMethod { get; set; }


        public virtual Supplier Supplier { get; set; }

        public virtual BankAccount BankAccount { get; set; }

        public virtual Upload Upload { get; set; }
    }
}