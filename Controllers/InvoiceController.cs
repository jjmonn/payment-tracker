﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CsvHelper;
using EcheancierDotNet.DAL;
using EcheancierDotNet.Models;
using EcheancierDotNet.ViewModels;
using PerpetuumSoft.Knockout;

namespace EcheancierDotNet.Controllers
{
    public class InvoiceController : KnockoutController
    {
        public string m_message = "";

        private PaymentContext db = new PaymentContext();

        // GET: Invoice
        public ActionResult Index(string sortOrder)
        {
            //ViewBag.SupplierNameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            //ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";
            //ViewBag.AmountSortParm = String.IsNullOrEmpty(sortOrder) ? "amount_desc" : "";

            //var invoices = from s in db.Invoices
            //               select s;

            //switch (sortOrder)
            //{
            //    case "name_desc":
            //        invoices = invoices.OrderByDescending(s => s.Supplier.Name);
            //        break;
            //    case "Date":
            //        invoices = invoices.OrderBy(s => s.DueDate);
            //        break;
            //    case "date_desc":
            //        invoices = invoices.OrderByDescending(s => s.DueDate);
            //        break;
            //    case "amount":
            //        invoices = invoices.OrderBy(s => s.DueAmount);
            //        break;
            //    case "amount_desc":
            //        invoices = invoices.OrderByDescending(s => s.DueAmount);
            //        break;
            //    default:
            //        invoices = invoices.OrderBy(s => s.Supplier.Name);
            //        break;
            //}

            //return View(invoices.ToList());
            if (TempData.Count > 0)
            {
                ViewBag.Message = TempData["shortMessage"].ToString();
            }
            
            return View();
        }

        [HttpPost]
        public FileStreamResult Download()
        {
            List<Invoice> l_invoicesList;
            List<InvoiceCsv> l_wrappersList = new List<InvoiceCsv>();
            l_invoicesList = db.Invoices.Where(i => i.Paid == false && i.DueDate.Year > 2017 && i.Supplier.IsInterco == false).OrderBy(i => i.DueDate).ToList();

            if (l_invoicesList == null)
                return (null);

            foreach (Invoice c in l_invoicesList)
            {
                l_wrappersList.Add(new InvoiceCsv(c));
            }

            var result = WriteInvoiceCsvToMemory(l_wrappersList);
            var memoryStream = new MemoryStream(result);
            return new FileStreamResult(memoryStream, "text/csv") { FileDownloadName = "export_echeancier.csv" };

        }

        [HttpPost]
        public FileStreamResult DownloadPayments()
        {
            List<Invoice> l_invoicesList;
            List<PaymentCsv> l_wrappersList = new List<PaymentCsv>();
            l_invoicesList = db.Invoices.Where(i => i.Paid == false && i.ToBePaid == true).OrderBy(i => i.Supplier.Name).ToList();

            if (l_invoicesList == null)
                return (null);

            foreach (Invoice c in l_invoicesList)
            {
                l_wrappersList.Add(new PaymentCsv(c));
            }

            var result = WritePaymentCsvToMemory(l_wrappersList);
            var memoryStream = new MemoryStream(result);
            return new FileStreamResult(memoryStream, "text/csv") { FileDownloadName = "payments.csv" };

        }

        [HttpPost]
        public FileStreamResult DownloadPaymentsHistory()
        {
            List<Invoice> l_invoicesList;
            List<InvoiceCsv> l_wrappersList = new List<InvoiceCsv>();
            l_invoicesList = db.Invoices.Where(i => i.Paid == true).OrderBy(i => i.Supplier.Name).ToList();

            if (l_invoicesList == null)
                return (null);

            foreach (Invoice c in l_invoicesList)
            {
                l_wrappersList.Add(new InvoiceCsv(c));
            }

            var result = WriteInvoiceCsvToMemory(l_wrappersList);
            var memoryStream = new MemoryStream(result);
            return new FileStreamResult(memoryStream, "text/csv") { FileDownloadName = "payment_history.csv" };

        }


        public byte[] WriteInvoiceCsvToMemory(IEnumerable<InvoiceCsv> records)
        {
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(streamWriter))
            {
                csvWriter.Configuration.Delimiter = ";";
                csvWriter.WriteRecords(records);
                streamWriter.Flush();
                return memoryStream.ToArray();
            }
        }

        public byte[] WritePaymentCsvToMemory(IEnumerable<PaymentCsv> records)
        {
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(streamWriter))
            {
                csvWriter.Configuration.Delimiter = ";";
                csvWriter.WriteRecords(records);
                streamWriter.Flush();
                return memoryStream.ToArray();
            }
        }

        // GET: Payments
        public ActionResult Payments()
        {
            return View();
        }


        public string UploadInvoices(string p_csv_str, string p_uploadName)
        {
            var l_suppliers = db.Suppliers.ToList();
            var l_existing_doc_numbers = db.Invoices.Select(i => i.DocumentNumber).ToList();
            var l_existing_uploads_names = db.Upload.Select(i => i.Name).ToList();
            int l_nb_invoices_uploaded = 0;
            double l_total_amount_uploadedEUR = 0;
            double l_total_amount_uploadedUSD = 0;
            double l_total_amount_uploadedGBP = 0;

            Upload l_upload = new Upload(p_uploadName);
            InvoiceUploader l_uploader = new InvoiceUploader(l_suppliers, l_existing_doc_numbers);

            bool upload_result = l_uploader.ImportCSV(p_csv_str, l_upload.UploadID);
            if (upload_result == false)
            {
                string l_error_message = "";

                if (l_uploader.l_error_header == true)
                {
                    l_error_message = "Error in the columns of your upload file. Please contact Julien Monnereau, ALA France.";
                }
                else
                {
                    if (l_uploader.l_data_error != "")
                    {
                        l_error_message = l_uploader.l_data_error;
                    }
                    else
                    {
                        l_error_message = "Error during invoices upload, some suppliers are not in the list: ";
                        foreach (string l_supplier in l_uploader.m_suppliers_to_be_added)
                        {
                            l_error_message += " ; " + l_supplier;
                        }
                    }
                }
                return l_error_message;
            }
            else if (l_uploader.m_invoices_to_create.Count == 0)
            {
                return "No new invoice detected in the file.";
            }
            else
            {
                l_nb_invoices_uploaded = l_uploader.m_invoices_to_create.Count;
                try
                {
                    foreach (Invoice l_invoice in l_uploader.m_invoices_to_create)
                    {
                        if (ModelState.IsValid)
                        {
                            db.Invoices.Add(l_invoice);
                            switch (l_invoice.Currency)
                            {
                                case "EUR": l_total_amount_uploadedEUR += l_invoice.DueAmount ;
                                    break;
                                case "USD": l_total_amount_uploadedUSD += l_invoice.DueAmount;
                                    break;
                                case "GBP": l_total_amount_uploadedGBP += l_invoice.DueAmount;
                                    break;
                            }
                        }
                    }
                    db.Upload.Add(l_upload);
                    db.SaveChanges();
                    m_message = "Upload success, number of invoices uploaded: " + l_nb_invoices_uploaded.ToString();
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    return "Error in updating the database. The uplaoad failed.";
                }
                return "";
            }
        }

        public string UploadInvoiceTable(string p_csv_str)
        {
            var l_suppliers = db.Suppliers.ToList();
            var l_existing_doc_numbers = db.Invoices.Select(i => i.DocumentNumber).ToList();
         
            InvoiceUploader l_uploader = new InvoiceUploader(l_suppliers, l_existing_doc_numbers);

            bool upload_result = l_uploader.ImportCSVTable(p_csv_str);
            if (upload_result == false)
            {
                string l_error_message = "";
                if (l_uploader.l_data_error != "")
                {
                    l_error_message = l_uploader.l_data_error;
                }
                else
                {
                    l_error_message = "Error during invoices upload, some suppliers are not in the list: ";
                    foreach (string l_supplier in l_uploader.m_suppliers_to_be_added)
                    {
                        l_error_message += " ; " + l_supplier;
                    }
                }
                return l_error_message;
            }
            else if (l_uploader.m_invoices_to_create.Count == 0)
            {
                return "No new invoice detected in the file.";
            }
            else
            {
                try
                {
                    foreach (Invoice l_invoice in l_uploader.m_invoices_to_create)
                    {
                        if (ModelState.IsValid)
                        {
                            db.Invoices.Add(l_invoice);
                        }
                    }
                    db.SaveChanges();
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    return "Error in updating the database. The uplaoad failed.";
                }
                return "";
            }
        }

        // GET: Invoice/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // GET: Invoice/Create
        public ActionResult Create()
        {
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "Name");
            return View();
        }

        // POST: Invoice/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InvoiceID,SupplierID,Currency,DocumentNumber,DocumentReference,DocumentDate,DueDate,GoodsReceptionDate,RawAmount,VAT,DueAmount,ToBePaid,Paid,ProForma,Comment")] Invoice invoice)
        {
            if (invoice.UploadID == 0)
            {
                invoice.UploadID = 10; // Attention manual entry
            }

            if (ModelState.IsValid)
            {
                db.Invoices.Add(invoice);
                db.SaveChanges();
                TempData["shortMessage"] = "Invoice created successfully";
                return RedirectToAction("Index");
            }

            ViewBag.Alert = "Invoice not created, supplier ID:";
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "Name", invoice.SupplierID);
            return View(invoice);
        }

        // GET: Invoice/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "Name", invoice.SupplierID);
            return View(invoice);
        }

        // POST: Invoice/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InvoiceID,SupplierID,DocumentNumber,DocumentReference,DocumentDate,DueDate,GoodsReceptionDate,RawAmount,VAT,DueAmount,ToBePaid,Paid,ProForma,Comment")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invoice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SupplierID = new SelectList(db.Suppliers, "ID", "Name", invoice.SupplierID);
            return View(invoice);
        }

        // GET: Invoice/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invoice invoice = db.Invoices.Find(id);
            if (invoice == null)
            {
                return HttpNotFound();
            }
            return View(invoice);
        }

        // POST: Invoice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invoice invoice = db.Invoices.Find(id);
            db.Invoices.Remove(invoice);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
