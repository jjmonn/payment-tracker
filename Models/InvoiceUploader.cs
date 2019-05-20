using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using EcheancierDotNet.Models;
using EcheancierDotNet.Controllers;
using System.IO;

namespace EcheancierDotNet.Models
{

    public class InvoiceUploader
    {

        public string[] uploadHeaders;
        public string[][] data;
        private List<Supplier> m_suppliersList;
        public List<Invoice> m_invoices_to_create { get; }
        private List<string> m_existing_doc_number_list;
        public List<string> m_suppliers_to_be_added;
        public bool l_error_header = false;
        public string l_data_error = "";

        public InvoiceUploader(List<Supplier> p_suppliersList, List<string> p_doc_number_list)
        {
            m_invoices_to_create = new List<Invoice>();
            m_suppliers_to_be_added = new List<string>();

            uploadHeaders = new string[]
             {
              "Account","Vendor Name","Document type","Reference","Document Date","Document currency","Amount in doc. curr.",
              "Clearing Document","Document Number","Text","Document Header Text","Net due date","Payment block","Payment Method"
            };

            m_suppliersList = p_suppliersList;
            m_existing_doc_number_list = p_doc_number_list;

        }

        internal bool ImportCSV(string p_path, int p_uploadID)
        {
            string csvData = File.ReadAllText(p_path);
            bool l_result = true;
            int i = 0;

            //Rows Loop  
            foreach (string l_row in csvData.Split('\n'))
            {
                if (!string.IsNullOrEmpty(l_row))
                {
                    if (i == 0) {
                        if (CheckFileHeaders(l_row) == false)
                        {
                            l_error_header = true;
                            return false;
                        }
                    }
                    else
                    {
                        string l_row2 = l_row.Replace("\r", "");
                        string[] l_str = l_row2.Split(';');

                        if (l_str[8] != "")                       // col 8 => DOC NUMBER
                        {
                            string l_doc_number = l_str[8];       // col 8 => DOC NUMBER

                            if (!m_existing_doc_number_list.Contains(l_doc_number))
                            {
                                bool l_creation_result = AddInvoiceToList(l_str, p_uploadID);
                                if (l_creation_result == false) { l_result = false; }
                            }
                        }
                    }
                }
                i++;
            }

            // Attention : retourner les erreurs (actuellement liste des frs non créés dispo)
            // refus ou acceptation de l'import
            // quid création de nouveaux fournisseurs => return false with message frs to be created
            
            return l_result;
        }

        internal bool ImportCSVTable(string p_path)
        {
            string csvData = File.ReadAllText(p_path);
            bool l_result = true;
            int i = 0;

            //Rows Loop  
            foreach (string l_row in csvData.Split('\n'))
            {
                if (!string.IsNullOrEmpty(l_row))
                {
                    string l_row2 = l_row.Replace("\r", "");
                    string[] l_str = l_row2.Split(';');

                    if (l_str[3] != "")                             // col 3 => DOC NUMBER
                    {
                        string l_doc_number = l_str[3];             // col 3 => DOC NUMBER

                        if (!m_existing_doc_number_list.Contains(l_doc_number))
                        {
                            bool l_creation_result = AddInvoiceTuppleToList(l_str);
                            if (l_creation_result == false) { l_result = false; }
                        }
                    }
                }
                i++;
            }
            return l_result;
        }


        private bool CheckFileHeaders(string p_headers)
        {
            int j = 0;
            p_headers = p_headers.Replace("\r", "");

            foreach (string p_value in p_headers.Split(';'))
            {
                if (p_value != uploadHeaders[j])
                {
                    return false;
                }
                j++;
            }
            return true;
        }

        private bool AddInvoiceToList(string[] p_str, int p_uploadID)
        {
            Boolean l_result = true;

            if (p_str[0] != "")
            {
                int l_sap_code = Convert.ToInt32(p_str[0]);
             
                Supplier l_supplier = GetSupplierBySAPCode(l_sap_code);

                if (l_supplier != null)
                {
                    if (l_supplier.IsProForma == false)
                    {
                        Invoice l_newInvoice = GetNewInvoice(l_supplier, p_str, p_uploadID);
                        if (l_newInvoice != null)
                        {
                            m_invoices_to_create.Add(l_newInvoice);
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    if (!m_suppliers_to_be_added.Contains(l_sap_code.ToString()))
                    {
                        m_suppliers_to_be_added.Add(l_sap_code.ToString());
                    }
                    l_result = false;
                }
            }
            return l_result;
        }

        private bool AddInvoiceTuppleToList(string[] p_str)
        {
            Boolean l_result = true;

            if (p_str[1] != "")
            {
                int l_supplierID = Convert.ToInt32(p_str[1]);
                Supplier l_supplier = GetSupplierByID(l_supplierID);

                if (l_supplier != null)
                {
                    Invoice l_newInvoice = GetNewInvoiceTableFormat(p_str);
                    if (l_newInvoice != null)
                    {
                        m_invoices_to_create.Add(l_newInvoice);
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    if (!m_suppliers_to_be_added.Contains(p_str[1]))
                    {
                        m_suppliers_to_be_added.Add(p_str[1]);
                    }
                    l_result = false;
                }
            }
            return l_result;
        }

        private Invoice GetNewInvoice(Supplier l_supplier, string[] p_str, int p_uploadID)
        {
            Invoice l_invoice = new Invoice();

            try
            {
                l_invoice.SupplierID = l_supplier.ID; //    l_row[1]   // retreive Supplier's ID from Name
                l_invoice.Currency = p_str[5];
                l_invoice.DocumentNumber = p_str[8];
                l_invoice.DocumentReference = p_str[3];
                l_invoice.DocumentHeader = p_str[10];
                l_invoice.DocumentDate = DateTime.Parse(p_str[4].Replace('/', '-'));
                l_invoice.DueDate = DateTime.Parse(p_str[11].Replace('/', '-'));
                l_invoice.GoodsReceptionDate = null;
                l_invoice.RawAmount = 0;
                l_invoice.VAT = 0;
                l_invoice.DueAmount = -Convert.ToDouble(p_str[6]);
                l_invoice.ToBePaid = false;
                l_invoice.Paid = false;
                l_invoice.ProForma = false;
                l_invoice.Comment = "";
                l_invoice.BankID = 0;           
                l_invoice.PaymentDate = null;   
                l_invoice.UploadID = p_uploadID;
                l_invoice.PaymentMethod = 0;
            }
            catch (Exception e)
            {
                l_data_error = "Error in the format of your data, please check " + e.Message;
            }       
            return l_invoice;
        }

        private Invoice GetNewInvoiceTableFormat(string[] p_str)
        {
            Invoice l_invoice = new Invoice();

            try
            {
                l_invoice.SupplierID = Convert.ToInt32(p_str[1]);                       //    l_row[1]   // retreive Supplier's ID from Name
                l_invoice.Currency = p_str[2];                                          //    l_row[2]
                l_invoice.DocumentNumber = p_str[3];                                    //    l_row[3]
                l_invoice.DocumentReference = p_str[4];                                 //    l_row[4]
                l_invoice.DocumentHeader = p_str[5];                                    //    l_row[5]
                l_invoice.DocumentDate = DateTime.Parse(p_str[6].Replace('/', '-'));    //    l_row[6]
                l_invoice.DueDate = DateTime.Parse(p_str[7].Replace('/', '-'));         //    l_row[7]
                l_invoice.GoodsReceptionDate = null;                                    //    l_row[8]
                l_invoice.RawAmount = 0;                                                //    l_row[9]
                l_invoice.VAT = 0;                                                      //    l_row[10]
                l_invoice.DueAmount = Convert.ToDouble(p_str[11]);                      //    l_row[11]
                l_invoice.ToBePaid = Convert.ToBoolean(Convert.ToInt16(p_str[12]));     //    l_row[12]
                l_invoice.Paid = Convert.ToBoolean(Convert.ToInt16(p_str[13]));         //    l_row[13]
                l_invoice.ProForma = Convert.ToBoolean(Convert.ToInt16(p_str[14]));     //    l_row[14]
                l_invoice.Comment = p_str[14];                                          //    l_row[15]
                l_invoice.BankID = 0;               // à voir dans le futur             //    l_row[16]
                l_invoice.PaymentDate = null;                                           //    l_row[17]
                l_invoice.UploadID = 1;             // dans le futur à voir             //    l_row[18]
                l_invoice.PaymentMethod = 0;                                            //    l_row[20]
            }
            catch (Exception e)
            {
                l_data_error = "Error in the format of your data, please check " + e.Message;
            }

            return l_invoice;
        }


        private Supplier GetSupplierBySAPCode(int p_sap_code)
        {

            foreach (Supplier l_supplier in m_suppliersList)
            {
                if (l_supplier.SAPAccountNumber == p_sap_code)
                {
                    return l_supplier;
                }
            }
            return null;
        }

        private Supplier GetSupplierByID(int p_supplierID)
        {

            foreach (Supplier l_supplier in m_suppliersList)
            {
                if (l_supplier.ID == p_supplierID)
                {
                    return l_supplier;
                }
            }
            return null;
        }


    }
}