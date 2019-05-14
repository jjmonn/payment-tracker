using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcheancierDotNet.Models;

namespace EcheancierDotNet.ViewModels
{
     public class SupplierWrapper
    {
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
         public string Currency { get; set; }
         public Boolean IsProforma { get; set; }
         public Boolean IsInterco { get; set; }
         public int PaymentDelay { get; set; }


         public List<InvoiceWrapper> Invoices { get; }
         public List<Total> Totals { get; set; }

         public SupplierWrapper(Supplier l_supplier, bool l_toBePaid_filter = false)
        {
            this.ID = l_supplier.ID;
            this.SAPAccountNumber = l_supplier.SAPMainAccountNumber;
            this.SAPMainAccountNumber = l_supplier.SAPMainAccountNumber;
            this.Name = l_supplier.Name;
            this.Bank = l_supplier.Bank;
            this.BankCode = l_supplier.BankCode;
            this.Guichet = l_supplier.Guichet;
            this.AccountNumber = l_supplier.AccountNumber;
            this.BankKey = l_supplier.BankKey;
            this.BankAddress = l_supplier.BankAddress;
            this.BIC = l_supplier.BIC;
            this.IBAN = l_supplier.IBAN;
            this.BankCountry = l_supplier.BankCountry;
            this.Currency = l_supplier.Currency;
            this.IsProforma = l_supplier.IsProForma;
            this.IsInterco = l_supplier.IsInterco;
            this.PaymentDelay = l_supplier.PaymentDelay;

            this.Invoices = new List<InvoiceWrapper>();

            double l_totalEUR = 0;
            double l_totalUSD = 0;
            double l_totalGBP = 0;

            string l_EUR_references = "";
            string l_USD_references = "";
            string l_GBP_references = "";

            Totals = new List<Total>();

            foreach (Invoice l_invoice in l_supplier.Invoices)
            {
                if (l_toBePaid_filter == true)
                {
                    

                    if (l_invoice.ToBePaid == true)
                    {
                        Invoices.Add(new InvoiceWrapper(l_invoice));

                        switch (l_invoice.Currency)
                        {
                            case "EUR":
                                l_totalEUR += l_invoice.DueAmount;
                                l_EUR_references = l_EUR_references + ';' + l_invoice.DocumentReference;  
                            break;

                            case "USD":
                                l_totalUSD += l_invoice.DueAmount;
                                l_USD_references = l_USD_references + ';' + l_invoice.DocumentReference;
                                break;

                            case "GBP":
                                l_totalGBP += l_invoice.DueAmount;
                                l_GBP_references = l_GBP_references + ';' + l_invoice.DocumentReference;
                                break;
                        }
                    }
                }
                else
                {
                    Invoices.Add(new InvoiceWrapper(l_invoice));
                }
            }
            Totals.Add(new Total("EUR", l_totalEUR, l_EUR_references));
            Totals.Add(new Total("USD", l_totalUSD, l_USD_references));
            Totals.Add(new Total("GBP", l_totalGBP, l_GBP_references));
        }

    }
}