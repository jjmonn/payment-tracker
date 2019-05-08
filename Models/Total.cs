using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcheancierDotNet.Models
{
    public class Total
    {
        public string Currency { get; set; }
        public string References { get; set; }
        public double Amount { get; set; }


        public Total(string l_currency, double l_amount, string l_references)
        {
            this.Currency = l_currency;
            this.Amount = l_amount;
            this.References = l_references;
        }
    }
}