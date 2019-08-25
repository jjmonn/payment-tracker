using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Xml;

namespace EcheancierDotNet.Remises
{
    public class SEPAWireGenerator
    {

        public const int MAX_REFERENCE_LENGHT = 12;
        public const int MAX_PAYMENT_INFO_LENGHT = 35; 


        static void WriteXml(string sOutputFileName,
                              string p_date,
                              string p_date_iso,
                              string p_date_tiret,
                              string p_hour,
                              string p_amount,
                              string p_supplier_name,
                              string p_reference,
                              string p_libelle,
                              int p_nb_transactions,
                              string p_ala_bank_IBAN,
                              string p_ala_BIC,
                              string p_currency,
                              string p_supplier_bic,
                              string p_supplier_country_code,
                              string p_supplier_city,
                              string p_supplier_ad1,
                              string p_supplier_iban)
        {

            string l_payment_info;

            p_reference = p_reference.Substring(0, MAX_REFERENCE_LENGHT);
            l_payment_info = p_supplier_name + " - " + p_reference;
            l_payment_info = l_payment_info.Substring(0, MAX_PAYMENT_INFO_LENGHT);


            XmlWriter xmlWriter = XmlWriter.Create("test.xml");

            //"<?xml version=""1.0""?>"      '"<?xml version=" & Q & "1.0" & Q & " encoding=" & Q & "UTF-8" & Q & "?>"
            // check indent size

            xmlWriter.WriteStartDocument();
           
            xmlWriter.WriteStartElement("Document");
                xmlWriter.WriteAttributeString("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
                xmlWriter.WriteAttributeString("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
                xmlWriter.WriteAttributeString("xmlns", "urn:iso:std:iso:20022:tech:xsd:pain.001.001.03");

                xmlWriter.WriteStartElement("CstmrCdtTrfInitn");

                    xmlWriter.WriteStartElement("GrpHdr");

                        xmlWriter.WriteStartElement("MsgId");
                        xmlWriter.WriteString(p_date + " " + p_hour + " ALA ADVANCED LO");
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("CreDtTm");
                        xmlWriter.WriteString(p_date_iso);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("NbOfTxs");
                        xmlWriter.WriteString("1");
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("CtrlSum");
                        xmlWriter.WriteString(p_amount);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("InitgPty");
                            xmlWriter.WriteStartElement("Nm");
                            xmlWriter.WriteString("ALA ADVANCED LOGISTICS FOR AEROSPAC");
                            xmlWriter.WriteEndElement();
                        xmlWriter.WriteEndElement();    // close InitgPty
                    xmlWriter.WriteEndElement();    // close GrpHdr

                    xmlWriter.WriteStartElement("PmtInf");

                        xmlWriter.WriteStartElement("PmtInfId");
                        xmlWriter.WriteString(l_payment_info);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("PmtMtd");
                        xmlWriter.WriteString("TRF");
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("BtchBookg");
                        xmlWriter.WriteString("true");
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("NbOfTxs");
                        xmlWriter.WriteString(p_nb_transactions.ToString());
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("CtrlSum");
                        xmlWriter.WriteString(p_amount);
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("PmtTpInf");
                            xmlWriter.WriteStartElement("SvcLvl");
                                xmlWriter.WriteStartElement("Cd");
                                xmlWriter.WriteString("SEPA");
                                xmlWriter.WriteEndElement();
                            xmlWriter.WriteEndElement();    //end SvcLvl
                            xmlWriter.WriteStartElement("CtgyPurp");
                                xmlWriter.WriteStartElement("Cd");
                                xmlWriter.WriteString("SUPP");
                                xmlWriter.WriteEndElement();
                            xmlWriter.WriteEndElement();    //end CtgyPurp
                        xmlWriter.WriteEndElement();    //end PmtTpInf

                        xmlWriter.WriteStartElement("ReqdExctnDt");
                        xmlWriter.WriteString("l_date_tiret");
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("Dbtr");
                            xmlWriter.WriteStartElement("Nm");
                            xmlWriter.WriteString("ALA ADVANCED LOGISTICS FOR AEROSPAC");
                            xmlWriter.WriteEndElement();
                            xmlWriter.WriteStartElement("PstlAdr");
                                xmlWriter.WriteStartElement("Ctry");
                                xmlWriter.WriteString("FR");
                                xmlWriter.WriteEndElement();
                            xmlWriter.WriteEndElement();    //end PstlAdr
                        xmlWriter.WriteEndElement();    //end Dbtr
                        xmlWriter.WriteStartElement("DbtrAcct");
                            xmlWriter.WriteStartElement("Id");
                                xmlWriter.WriteStartElement("IBAN");
                                xmlWriter.WriteString(p_ala_bank_IBAN);
                                xmlWriter.WriteEndElement();
                            xmlWriter.WriteEndElement();    //end Id
                        xmlWriter.WriteEndElement();    //end DbtrAcct
                        xmlWriter.WriteStartElement("DbtrAgt");
                            xmlWriter.WriteStartElement("FinInstnId");
                                xmlWriter.WriteStartElement("BIC");
                                xmlWriter.WriteString(p_ala_BIC);
                                xmlWriter.WriteEndElement();
                            xmlWriter.WriteEndElement();    //end FinInstnId
                        xmlWriter.WriteEndElement();    //end DbtrAgt

                        xmlWriter.WriteStartElement("ChrgBr");
                        xmlWriter.WriteString("SLEV");
                        xmlWriter.WriteEndElement();

                        xmlWriter.WriteStartElement("CdtTrfTxInf");
                            xmlWriter.WriteStartElement("PmtId");
                                xmlWriter.WriteStartElement("EndToEndId");
                                xmlWriter.WriteString(p_reference);
                                xmlWriter.WriteEndElement();
                            xmlWriter.WriteEndElement();    //end PmtId
                            xmlWriter.WriteStartElement("Amt");
                                xmlWriter.WriteStartElement("InstdAmt");
                                xmlWriter.WriteAttributeString("Ccy", p_currency);
                                xmlWriter.WriteString(p_amount);
                                xmlWriter.WriteEndElement();
                            xmlWriter.WriteEndElement();    //end Amt

                            xmlWriter.WriteStartElement("CdtrAgt");
                                xmlWriter.WriteStartElement("FinInstnId");
                                    xmlWriter.WriteStartElement("BIC");
                                    xmlWriter.WriteString(p_supplier_bic);
                                    xmlWriter.WriteEndElement();
                                xmlWriter.WriteEndElement();    //end FinInstnId
                            xmlWriter.WriteEndElement();    //end CdtrAgt
                            xmlWriter.WriteStartElement("Cdtr");
                                xmlWriter.WriteStartElement("Nm");
                                xmlWriter.WriteString(p_supplier_name);
                                xmlWriter.WriteEndElement();
                                xmlWriter.WriteStartElement("PstlAdr");
                                    xmlWriter.WriteStartElement("Ctry");
                                    xmlWriter.WriteString(p_supplier_country_code);
                                    xmlWriter.WriteEndElement();
                                    xmlWriter.WriteStartElement("AdrLine");
                                    xmlWriter.WriteString(p_supplier_city);
                                    xmlWriter.WriteEndElement();
                                    xmlWriter.WriteStartElement("AdrLine");
                                    xmlWriter.WriteString(p_supplier_ad1);
                                    xmlWriter.WriteEndElement();
                                xmlWriter.WriteEndElement();    //end PstlAdr
                            xmlWriter.WriteEndElement();    //end Cdtr
                            xmlWriter.WriteStartElement("CdtrAcct");
                                xmlWriter.WriteStartElement("Id");
                                    xmlWriter.WriteStartElement("IBAN");
                                    xmlWriter.WriteString(p_supplier_iban);
                                    xmlWriter.WriteEndElement();
                                xmlWriter.WriteEndElement();    //end Id
                            xmlWriter.WriteEndElement();    //end CdtrAcct
                            xmlWriter.WriteStartElement("RmtInf");
                                xmlWriter.WriteStartElement("Ustrd");
                                xmlWriter.WriteString(p_libelle);
                                xmlWriter.WriteEndElement();
            
            xmlWriter.WriteEndDocument();
            xmlWriter.Close();
        }


    }
}