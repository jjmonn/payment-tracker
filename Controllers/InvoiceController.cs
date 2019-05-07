﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EcheancierDotNet.DAL;
using EcheancierDotNet.Models;
using PerpetuumSoft.Knockout;

namespace EcheancierDotNet.Controllers
{
    public class InvoiceController : KnockoutController
    {
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

            return View();
        }


        // GET: Payments
        public ActionResult Payments()
        {
            return View();
        }


        // This action handles the form POST and the upload
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the filename
                var fileName = Path.GetFileName(file.FileName);
                var p_path = Path.GetFullPath(file.FileName);

                // store the file inside ~/App_Data/uploads folder
                // avoid to store upload 
                var path = Path.Combine(Server.MapPath("~/"), fileName);
                file.SaveAs(path);

                var l_suppliers = db.Suppliers.ToList();
                var l_existing_doc_numbers = db.Invoices.Select(i => i.DocumentNumber).ToList();

                InvoiceUploader l_uploader = new InvoiceUploader(l_suppliers, l_existing_doc_numbers);

                bool upload_result = l_uploader.ImportCSV(path);
                if (upload_result == false)
                {
                    ViewBag.Message = "Error during invoices upload, some suppliers are not in the list";
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
                            db.SaveChanges();
                        }
                    }
                    catch (DataException /* dex */)
                    {
                        //Log the error (uncomment dex variable name and add a line here to write a log.
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    }
                    ViewBag.Message = "Invoices upload succeeded";
                }

            }
            // send back to index page with message : success ; error ; ok duplicates 
            return RedirectToAction("Index");
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
        public ActionResult Create([Bind(Include = "InvoiceID,SupplierID,DocumentNumber,DocumentReference,DocumentDate,DueDate,GoodsReceptionDate,RawAmount,VAT,DueAmount,ToBePaid,Paid,ProForma,Comment")] Invoice invoice)
        {
            if (ModelState.IsValid)
            {
                db.Invoices.Add(invoice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

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
