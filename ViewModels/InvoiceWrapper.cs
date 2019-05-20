using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcheancierDotNet.Models;

namespace EcheancierDotNet.ViewModels
{
    public class InvoiceWrapper
    {
        public int InvoiceID { get; set; }
        public int SupplierID { get; set; }
        public string Currency { get; set; }
        public string SupplierName { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentReference { get; set; }
        public DateTime DocumentDate { get; set; }
        public DateTime DueDate { get; set; }
        public Nullable<DateTime> GoodsReceptionDate { get; set; }
        public double RawAmount { get; set; }
        public double VAT { get; set; }
        public double DueAmount { get; set; }
        public bool ToBePaid { get; set; }
        public bool Paid { get; set; }
        public bool IsSupplierInterco { get; set; }
        public bool ProForma { get; set; }
        public string Comment { get; set; }
        public string BankAccountName { get; set; }
        public int BankAccountID { get; set; }
        public Nullable<DateTime> PaymentDate { get; set; }
        public int UploadID { get; set; }
        public int PaymentMethod { get; set; }



        public InvoiceWrapper(Invoice p_invoice)
        {
            this.InvoiceID = p_invoice.InvoiceID;
            this.SupplierID = p_invoice.SupplierID;
            this.Currency = p_invoice.Currency;
            this.SupplierName = p_invoice.Supplier.Name;
            this.DocumentNumber = p_invoice.DocumentNumber;
            this.DocumentReference = p_invoice.DocumentReference;
            this.DocumentDate = p_invoice.DocumentDate;
            this.DueDate = p_invoice.DueDate;
            this.GoodsReceptionDate = p_invoice.GoodsReceptionDate;
            this.RawAmount= p_invoice.RawAmount;
            this.VAT = p_invoice.VAT;
            this.DueAmount = p_invoice.DueAmount;
            this.ToBePaid = p_invoice.ToBePaid;
            this.Paid = p_invoice.Paid;
            this.IsSupplierInterco = p_invoice.Supplier.IsInterco;
            this.ProForma = p_invoice.ProForma;
            this.Comment = p_invoice.Comment;
            this.PaymentDate = p_invoice.PaymentDate;
            this.UploadID = p_invoice.UploadID;
            this.PaymentMethod = p_invoice.PaymentMethod;
            //this.BankAccountID = p_invoice.BankID;
            //this.BankAccountName = p_invoice.BankAccount.Name;
        }

    }
}