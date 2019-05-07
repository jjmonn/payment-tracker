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
        public List<Supplier> m_suppliers_to_create; 

        public SupplierUploader(List<Supplier> p_suppliers_list)
        {
            m_suppliers_codes_list = new List<int>();
            m_suppliers_to_create = new List<Supplier>();

            foreach (Supplier l_supplier in p_suppliers_list){
                m_suppliers_codes_list.Add(l_supplier.SAPAccountNumber);
            }
        }

        internal bool ImportCSV(string p_path)
        {
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
                            l_supplier.PaymentDelay = 0;

                            m_suppliers_to_create.Add(l_supplier);

                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            return true;
        }



    }
}