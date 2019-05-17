using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcheancierDotNet.Models
{
    public class Flow
    {
        public int ID { get; set; }
        public int SupplierID { get; set; }
        public string Currency { get; set; }
        public string DocumentReference { get; set; }
        public string DocumentHeader { get; set; }
        public DateTime DueDate { get; set; }
        public double RawAmount { get; set; }
        public double VAT { get; set; }
        public double DueAmount { get; set; }
        public string Comment { get; set; }
        public int BankID { get; set; }

    }
}