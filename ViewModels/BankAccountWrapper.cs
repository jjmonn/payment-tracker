using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EcheancierDotNet.Models;

namespace EcheancierDotNet.ViewModels
{
    public class BankAccountWrapper
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
        public double MaxOverdraft { get; set; }
        public Boolean DefaultBank { get; set; }
        public double AvailableCash { get; set; }
        public double Payments { get; set; }
        public double CashAfterPayments { get; set; }
    


        public BankAccountWrapper(BankAccount p_bankAccount)
        {
            this.ID = p_bankAccount.ID;
            this.Name = p_bankAccount.Name;
            this.BankCode = p_bankAccount.BankCode;
            this.AgencyCode = p_bankAccount.AgencyCode;
            this.AccountNumber = p_bankAccount.AccountNumber;
            this.RIBKey = p_bankAccount.RIBKey;
            this.BIC = p_bankAccount.BIC;
            this.IBAN = p_bankAccount.IBAN;
            this.Currency = p_bankAccount.Currency;
            this.BankAddress = p_bankAccount.BankAddress;
            this.BankCountry = p_bankAccount.BankCountry;
            this.Balance = p_bankAccount.Balance;
            this.MaxOverdraft = p_bankAccount.MaxOverdraft;
            this.DefaultBank = p_bankAccount.DefaultBank;
            this.AvailableCash = 0;
            this.Payments = 0;
            this.CashAfterPayments = 0;
        }

    }
}