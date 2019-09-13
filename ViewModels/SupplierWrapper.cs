using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcheancierDotNet.Models;

namespace EcheancierDotNet.ViewModels
{

     public class SupplierWrapper
     {
         const string REF_SEPERATOR = " ";

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
         public bool ToBePaidFlag { get; } 


         //public Dictionary<string, List<InvoiceWrapper>> InvoicesDic { get; }
         public List<InvoiceWrapper> Invoices { get; }
         public List<Total> Totals { get; set; }

         public SupplierWrapper(Supplier l_supplier, List<BankAccount> p_bankAccounts, bool l_toBePaid_filter = false)
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
            this.ToBePaidFlag = false;

            this.Invoices = new List<InvoiceWrapper>();
            Dictionary<string, List<InvoiceWrapper>> l_invoicesDic = new Dictionary<string, List<InvoiceWrapper>>();
        
            List<InvoiceWrapper> l_invoicesEUR = new List<InvoiceWrapper>();
            List<InvoiceWrapper> l_invoicesUSD = new List<InvoiceWrapper>();
            List<InvoiceWrapper> l_invoicesGBP = new List<InvoiceWrapper>();

            double l_totalEUR = 0;
            double l_totalUSD = 0;
            double l_totalGBP = 0;

            string l_EUR_references = "";
            string l_USD_references = "";
            string l_GBP_references = "";

            Totals = new List<Total>();

            // Those computations should go in payments.js code
            foreach (Invoice l_invoice in l_supplier.Invoices.OrderBy(i=> i.DueDate))
            {
                if (l_toBePaid_filter == true)
                {
                    if (l_invoice.ToBePaid == true && l_invoice.Paid == false)
                    {
                        this.ToBePaidFlag = true;

                        //l_invoicesDic[l_invoice.Currency].Add(new InvoiceWrapper(l_invoice));      runtime error on azure

                        switch (l_invoice.Currency)
                        {
                            case "EUR":
                                l_invoicesEUR.Add(new InvoiceWrapper(l_invoice));
                                l_totalEUR += l_invoice.DueAmount;
                                l_EUR_references = l_EUR_references + REF_SEPERATOR + l_invoice.DocumentReference;  
                            break;

                            case "USD":
                                l_invoicesUSD.Add(new InvoiceWrapper(l_invoice));
                                l_totalUSD += l_invoice.DueAmount;
                                l_USD_references = l_USD_references + REF_SEPERATOR + l_invoice.DocumentReference;
                                break;

                            case "GBP":
                                l_invoicesGBP.Add(new InvoiceWrapper(l_invoice));
                                l_totalGBP += l_invoice.DueAmount;
                                l_GBP_references = l_GBP_references + REF_SEPERATOR + l_invoice.DocumentReference;
                                break;
                        }
                    }
                }
                else
                {
                    Invoices.Add(new InvoiceWrapper(l_invoice));
                }
            }
            if (l_totalEUR != 0) {
                Totals.Add(new Total("EUR", l_totalEUR, l_EUR_references, l_invoicesEUR, GetDefaultBankAccount("EUR", p_bankAccounts),l_supplier.ID));
            }
            if (l_totalUSD != 0) {
                Totals.Add(new Total("USD", l_totalUSD, l_USD_references, l_invoicesUSD, GetDefaultBankAccount("USD", p_bankAccounts), l_supplier.ID));
            }
            if (l_totalGBP != 0){
                Totals.Add(new Total("GBP", l_totalGBP, l_GBP_references, l_invoicesGBP, GetDefaultBankAccount("GBP", p_bankAccounts), l_supplier.ID));
            }
        }

        private BankAccount GetDefaultBankAccount(string p_currency, List<BankAccount> p_bankAccounts)
        {
            foreach (BankAccount l_bankAccount in p_bankAccounts)
            {
                if (l_bankAccount.Currency == p_currency && l_bankAccount.DefaultBank == true)
                {
                    return l_bankAccount;
                }
            }
            return null;
        }
    }
}