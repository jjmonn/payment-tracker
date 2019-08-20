using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EcheancierDotNet.Models
{
    public enum SupplierPaymentArea
    {
        SEPA = 0,
        INT = 1
    }


    public class TurboSupplier
    {
        public int TurboSupplierID { get; set; }
        public int PaymentArea { get; set; }
        public string TYPE { get; set; }
        public int BENEF_CODE { get; set; }
        public string BENEF_RS { get; set; }
        public string SIREN { get; set; }
        public string NOM { get; set; }
        public string BENEF_AD1 { get; set; }
        public string BENEF_CP { get; set; }
        public string BENEF_VILLE { get; set; }
        public string BENEF_MAIL { get; set; }
        public string BENEF_CODEPAYS { get; set; }
        public string DOB_IBAN { get; set; }
        public string BQE_BIC { get; set; }
        public string BQE_NOM { get; set; }
        public string BQE_CODEPAYS { get; set; }
        public string GROUPE { get; set; }
        public double MONTANT { get; set; }
        public string DEVISE { get; set; }
        public string SOCIETE_BENEF { get; set; }
        public string DOB_TYPE { get; set; }

    }
}