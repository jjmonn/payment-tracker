using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace EcheancierDotNet.Models
{
    public class SupplierUploader
    {

        private List<int> m_suppliers_codes_list;
        private List<Supplier> m_suppliers;
        public List<Supplier> m_suppliers_to_create;
        public List<Supplier> m_suppliers_to_update;

        public SupplierUploader(List<Supplier> p_suppliers_list)
        {
            m_suppliers = p_suppliers_list;
            m_suppliers_codes_list = new List<int>();
            
            foreach (Supplier l_supplier in p_suppliers_list){
                m_suppliers_codes_list.Add(l_supplier.SAPAccountNumber);
            }
        }

        internal bool ImportCSV(string p_path)
        {
            m_suppliers_to_create = new List<Supplier>();

            string csvData = File.ReadAllText(p_path);
            foreach (string l_row in csvData.Split('\n'))
            {
                if (!string.IsNullOrEmpty(l_row))
                {
                    try
                    {
                        string l_row2 = l_row.Replace("\r", "");
                        string[] l_str = l_row2.Split(';');
                        int l_SAP_code = Convert.ToInt32(l_str[0]);

                        if (!m_suppliers_codes_list.Contains(l_SAP_code))
                        {
                            Supplier l_supplier = new Supplier();

                            l_supplier.SAPAccountNumber = l_SAP_code;
                            l_supplier.SAPMainAccountNumber = 0;
                            l_supplier.Name = l_str[1];
                            l_supplier.Bank = "";
                            l_supplier.BankCode = 0;
                            l_supplier.Guichet = 0;
                            l_supplier.AccountNumber = 0;
                            l_supplier.BankKey = 0;
                            l_supplier.BankAddress = "";
                            l_supplier.BIC = "";
                            l_supplier.IBAN = "";
                            l_supplier.BankCountry = "";
                            l_supplier.Currency = "";
                            l_supplier.IsProForma = (l_str[2] == "1");
                            l_supplier.IsInterco = (l_str[3] == "1");
                            l_supplier.PaymentDelay = 0;

                            m_suppliers_to_create.Add(l_supplier);

                        }
                    }
                    catch (Exception e)
                    {
                        // log exception
                    }
                }
            }
            return true;
        }

        internal bool UpdateSuppliers(string p_path)
        {
            m_suppliers_to_update = new List<Supplier>();

            string csvData = File.ReadAllText(p_path);
            foreach (string l_row in csvData.Split('\n'))
            {
                if (!string.IsNullOrEmpty(l_row))
                {
                    try
                    {
                        string l_row2 = l_row.Replace("\r", "");
                        string[] l_str = l_row2.Split(';');
                        int l_SAP_code = Convert.ToInt32(l_str[1]);

                           Supplier l_supplier = GetSupplierBySAPAccount(l_SAP_code);
                            if (l_supplier != null)
                            {
                                l_supplier.SAPAccountNumber = l_SAP_code;
                                // l_supplier.SAPMainAccountNumber = 0;                         // will be a free field
                                l_supplier.Name = l_str[3];                                     // String
                                l_supplier.Bank = l_str[4];                                     // String
                                l_supplier.BankCode = Convert.ToInt32(l_str[5]);                // int
                                l_supplier.Guichet = Convert.ToInt32(l_str[6]);                 // int
                                l_supplier.AccountNumber = Convert.ToInt32(l_str[7]);           // int
                                l_supplier.BankKey = Convert.ToInt32(l_str[8]);                 // int
                                l_supplier.BankAddress = l_str[9];                              // string
                                l_supplier.BIC = l_str[10];                                      // string
                                l_supplier.IBAN = l_str[11];                                     // string
                                l_supplier.BankCountry = l_str[12];                              // string
                                l_supplier.Currency = l_str[13];                                // string
                                l_supplier.IsProForma = (l_str[14] == "1");                     // bool
                                l_supplier.IsInterco = (l_str[15] == "1");                      // bool
                                l_supplier.PaymentDelay = Convert.ToInt32(l_str[16]);           // int

                            m_suppliers_to_update.Add(l_supplier);
                        }
                    }
                    catch (Exception e)
                    {
                        // log error e + SAP_account_number
                    }
                }
            }
            return true;
        }

        private Supplier GetSupplierBySAPAccount(int p_sap_account_number)
        {
            foreach (Supplier l_supp in m_suppliers)
            {
                if (l_supp.SAPAccountNumber == p_sap_account_number)
                {
                    return l_supp;
                }
            }
            return null;
        }


    }
}