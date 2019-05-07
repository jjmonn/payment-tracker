using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcheancierDotNet.Models
{
    public class Supplier
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; }
        public int SAPAccountNumber { get; set; }
        public int SAPMainAccountNumber { get; set; }
        public string Name { get; set; }
        public string Bank { get; set; }
        public int BankCode { get; set; }
        public int Guichet { get; set; }
        public int AccountNumber { get; set; }
        public int BankKey { get; set; }
        public string BankAddress { get; set; }
        public string BIC { get; set; }
        public string IBAN { get; set; }
        public string BankCountry { get; set; }
        public int PaymentDelay { get; set; }


        public virtual ICollection<Invoice> Invoices { get; set; }

    }
}