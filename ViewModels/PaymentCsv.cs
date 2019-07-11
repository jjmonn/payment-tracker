using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcheancierDotNet.Models;


namespace EcheancierDotNet.ViewModels
{
    public class PaymentCsv
    {
        public string Currency { get; set; }
        public int SupplierSAPAccountNumber { get; set; }
        public string SupplierName { get; set; }
        public string DocumentNumber { get; set; }
        public string DocumentReference { get; set; }
        public string DocumentDate { get; set; }
        public string DueDate { get; set; }
        public double DueAmount { get; set; }
        public string Comment { get; set; }
      
        public PaymentCsv(Invoice p_invoice)
        {
            this.Currency = p_invoice.Currency;
            this.SupplierSAPAccountNumber = p_invoice.Supplier.SAPAccountNumber;
            this.SupplierName = p_invoice.Supplier.Name;
            this.DocumentNumber = p_invoice.DocumentNumber;
            this.DocumentReference = p_invoice.DocumentReference;
            this.DocumentDate = p_invoice.DocumentDate.ToString("dd/MM/yyyy");
            this.DueDate = p_invoice.DueDate.ToString("dd/MM/yyyy");
            this.DueAmount = p_invoice.DueAmount;
            this.Comment = p_invoice.Comment;
        }


    }
}