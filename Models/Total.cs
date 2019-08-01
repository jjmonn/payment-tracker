using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcheancierDotNet.ViewModels;

namespace EcheancierDotNet.Models
{
    public class Total
    {
        public string Currency { get; set; }
        public string References { get; set; }
        public double Amount { get; set; }
        public bool Paid { get; set; }

        public List<InvoiceWrapper> Invoices { get; }

        public Total(string l_currency, double l_amount, string l_references, List<InvoiceWrapper> p_invoices)
        {
            this.Paid = false;
            this.Currency = l_currency;
            this.Amount = l_amount;
            this.References = l_references;
            this.Invoices = p_invoices;
        }
    }
}