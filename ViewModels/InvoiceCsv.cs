using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcheancierDotNet.Models;

namespace EcheancierDotNet.ViewModels
{
    public class InvoiceCsv
    {
        public int InvoiceID { get; set; }
        public string Currency { get; set; }
        public string SupplierName { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentReference { get; set; }
        public string DocumentDate { get; set; }
        public string DueDate { get; set; }
        public double DueAmount { get; set; }
        public bool ToBePaid { get; set; }
        public bool Paid { get; set; }
        public bool IsSupplierInterco { get; set; }
        public bool ProForma { get; set; }
        public string Comment { get; set; }
        public int UploadID { get; set; }
        public string PaymentDate { get; set; }
        public int SupplierSAPAccount { get; set; }


        public InvoiceCsv(Invoice p_invoice)
        {
            this.InvoiceID = p_invoice.InvoiceID;
            this.Currency = p_invoice.Currency;
            this.SupplierName = p_invoice.Supplier.Name;
            this.DocumentNumber = p_invoice.DocumentNumber;
            this.DocumentReference = p_invoice.DocumentReference;
            this.DocumentDate = p_invoice.DocumentDate.ToString("dd/MM/yyyy");
            this.DueDate = p_invoice.DueDate.ToString("dd/MM/yyyy");
            this.DueAmount = p_invoice.DueAmount;
            this.ToBePaid = p_invoice.ToBePaid;
            this.Paid = p_invoice.Paid;
            this.IsSupplierInterco = p_invoice.Supplier.IsInterco;
            this.ProForma = p_invoice.ProForma;
            this.Comment = p_invoice.Comment;
            this.UploadID = p_invoice.UploadID;
            this.PaymentDate = p_invoice.PaymentDate.HasValue ? p_invoice.PaymentDate.Value.ToString("dd/MM/yyyy") : string.Empty;
            this.SupplierSAPAccount = p_invoice.Supplier.SAPAccountNumber;
        }

    }
}