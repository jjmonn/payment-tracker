using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcheancierDotNet.Models
{
    public class BankAccount
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public int BankCode { get; set; }
        public int AgencyCode { get; set; }
        public int AccountNumber { get; set; }
        public int RIBKey { get; set; }
        public string BIC { get; set; }
        public string IBAN { get; set; }
        public string Currency { get; set; }
        public string BankAddress { get; set; }
        public string BankCountry { get; set; }
        public double Balance { get; set; }
        public Boolean DefaultBank { get; set; }

    }
}