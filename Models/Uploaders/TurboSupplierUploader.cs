using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


namespace EcheancierDotNet.Models.Uploaders
{
    public class TurboSupplierUploader
    {
        private List<TurboSupplier> m_suppliers;
        public List<TurboSupplier> m_suppliers_to_create;
        public List<TurboSupplier> m_suppliers_to_update;

        public TurboSupplierUploader(List<TurboSupplier> p_turbo_suppliers_list)
        {
            m_suppliers = p_turbo_suppliers_list;
            m_suppliers_to_create = new List<TurboSupplier>();
            m_suppliers_to_update = new List<TurboSupplier>();
        }

        internal bool CreateOrUpdateFromCsv(string p_path, SupplierPaymentArea p_paymentArea)
        {
            m_suppliers_to_create = new List<TurboSupplier>();

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

                        TurboSupplier l_turbo_supplier = GetSupplierBySAPAccount(l_SAP_code, p_paymentArea);

                        if (l_turbo_supplier == null)
                        {
                            l_turbo_supplier = new TurboSupplier();
                            FillTurboSupplierObject(l_turbo_supplier, l_str, p_paymentArea);
                            m_suppliers_to_create.Add(l_turbo_supplier);
                        }else
                        {
                            FillTurboSupplierObject(l_turbo_supplier, l_str, p_paymentArea);
                            m_suppliers_to_update.Add(l_turbo_supplier);
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

        private void FillTurboSupplierObject(TurboSupplier p_turbo_supplier, string[] p_str, SupplierPaymentArea p_paymentArea)
        {
            if (p_paymentArea == SupplierPaymentArea.SEPA)
            {
                FillSEPATurboSupplier(p_turbo_supplier, p_str);
            }
            else
            {
                FillINTERNATIONALTurboSupplier(p_turbo_supplier, p_str);
            }
        }

        private void FillSEPATurboSupplier(TurboSupplier p_turbo_supplier, string[] p_str)
        {
            p_turbo_supplier.PaymentArea = (int)SupplierPaymentArea.SEPA;
            p_turbo_supplier.TYPE = p_str[0];
            p_turbo_supplier.BENEF_CODE = Convert.ToInt32(p_str[1]);
            p_turbo_supplier.BENEF_RS = p_str[2];
            p_turbo_supplier.SIREN = p_str[3];
            p_turbo_supplier.NOM = p_str[4];
            p_turbo_supplier.BENEF_AD1 = p_str[5];
            p_turbo_supplier.BENEF_CP = p_str[6];
            p_turbo_supplier.BENEF_VILLE = p_str[7];
            p_turbo_supplier.BENEF_MAIL = p_str[8];
            p_turbo_supplier.BENEF_CODEPAYS = p_str[9];
            p_turbo_supplier.DOB_IBAN = p_str[10];
            p_turbo_supplier.BQE_BIC = p_str[11];
            p_turbo_supplier.BQE_NOM = p_str[12];
            p_turbo_supplier.BQE_CODEPAYS = p_str[13];
            p_turbo_supplier.GROUPE = p_str[14];
            p_turbo_supplier.MONTANT = Convert.ToDouble(p_str[15]);
            p_turbo_supplier.DEVISE = p_str[16];
            p_turbo_supplier.SOCIETE_BENEF = p_str[17];
            p_turbo_supplier.DOB_TYPE = p_str[18];
        }

        private void FillINTERNATIONALTurboSupplier(TurboSupplier p_turbo_supplier, string[] p_str)
        {
            p_turbo_supplier.PaymentArea = (int)SupplierPaymentArea.INT;
            p_turbo_supplier.TYPE = p_str[0];
            p_turbo_supplier.BENEF_CODE = Convert.ToInt32(p_str[1]);
            p_turbo_supplier.BENEF_RS = p_str[2];
            p_turbo_supplier.SIREN = p_str[3];
            p_turbo_supplier.NOM = p_str[4];
            p_turbo_supplier.BENEF_AD1 = p_str[5];
            p_turbo_supplier.BENEF_CP = p_str[6];
            p_turbo_supplier.BENEF_VILLE = p_str[7];
            p_turbo_supplier.BENEF_MAIL = p_str[8];
            p_turbo_supplier.BENEF_CODEPAYS = p_str[9];
            p_turbo_supplier.DOB_IBAN = p_str[11];
            p_turbo_supplier.BQE_BIC = p_str[12];
            p_turbo_supplier.BQE_NOM = p_str[13];
            p_turbo_supplier.BQE_CODEPAYS = p_str[14];
            p_turbo_supplier.GROUPE = "";
            p_turbo_supplier.MONTANT = 0;
            p_turbo_supplier.DEVISE = "";
            p_turbo_supplier.SOCIETE_BENEF = p_str[15];
            p_turbo_supplier.DOB_TYPE = p_str[10];
        }

        private TurboSupplier GetSupplierBySAPAccount(int p_sap_account_number, SupplierPaymentArea p_paymentArea)
        {
            foreach (TurboSupplier l_supp in m_suppliers)
            {
                if (l_supp.BENEF_CODE == p_sap_account_number && l_supp.PaymentArea == (int)p_paymentArea)
                {
                    return l_supp;
                }
            }
            return null;
        }

    }
}