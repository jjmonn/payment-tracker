using System;
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
    public class SupplierController : KnockoutController
    {
        private PaymentContext db = new PaymentContext();

        // GET: Supplier
        public ActionResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.AccountNumberSortParm = String.IsNullOrEmpty(sortOrder) ? "account_number_desc" : "account_number";
            var suppliers = from s in db.Suppliers
                           select s;
            switch (sortOrder)
            {
                case "name_desc":
                    suppliers = suppliers.OrderByDescending(s => s.Name);
                    break;
                case "account_number":
                    suppliers = suppliers.OrderBy(s => s.SAPAccountNumber);
                    break;
                case "account_number_desc":
                    suppliers = suppliers.OrderByDescending(s => s.SAPAccountNumber);
                    break;
                default:
                    suppliers = suppliers.OrderBy(s => s.Name);
                    break;
            }
            return View(suppliers.ToList());
        }


        // This action handles the form POST and the upload
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the filename
                var fileName = "upload.csv";    //Path.GetFileName(file.FileName);
                var file_path = Path.GetDirectoryName(file.FileName);

                // avoid to store upload 
                var p_path = Path.Combine(Server.MapPath("~/"), fileName);
                file.SaveAs(p_path);

                var l_suppliers = db.Suppliers.ToList();
                SupplierUploader l_uploader = new SupplierUploader(l_suppliers);

                bool upload_result = l_uploader.ImportCSV(p_path);
                if (upload_result == true)
                {
                    try
                    {
                        foreach (Supplier l_supplier in l_uploader.m_suppliers_to_create)
                        {
                            if (ModelState.IsValid)
                            {
                                db.Suppliers.Add(l_supplier);
                            }
                            db.SaveChanges();
                        }
                    }
                    catch (DataException /* dex */)
                    {
                        //Log the error (uncomment dex variable name and add a line here to write a log.
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    }
                }
            }
            // send back to index page with message : success ; error ; ok duplicates 
            return RedirectToAction("Index");
        }


        // GET: Supplier/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // GET: Supplier/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Supplier/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SAPAccountNumber,SAPMainAccountNumber,Name,Bank,BankCode,Guichet,AccountNumber,BankKey,BankAddress,BIC,IBAN,BankCountry,PaymentDelay")] Supplier supplier)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Suppliers.Add(supplier);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }

            return View(supplier);
        }

        // GET: Supplier/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Supplier/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult EditPost(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var supplierToUpdate = db.Suppliers.Find(id);
            if (TryUpdateModel(supplierToUpdate, "", new string[] { "SAPAccountNumber", "SAPMainAccountNumber", "Name", "Bank", "BankCode", "Guichet", "AccountNumber","BankKey",
                                                                    "BankAddress", "BIC", "IBAN", "BankCountry", "PaymentDelay" }))
            {
                try
                {
                    db.SaveChanges();

                    return RedirectToAction("Index");
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
        }
        return View(supplierToUpdate);
    }

        // GET: Supplier/Delete/5
        public ActionResult Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewBag.ErrorMessage = "Delete failed. Try again, and if the problem persists see your system administrator.";
            }
            Supplier supplier = db.Suppliers.Find(id);
            if (supplier == null)
            {
                return HttpNotFound();
            }
            return View(supplier);
        }

        // POST: Supplier/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            try
            {
                Supplier supplier = db.Suppliers.Find(id);
                db.Suppliers.Remove(supplier);
                db.SaveChanges();
            }
            catch (DataException/* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                return RedirectToAction("Delete", new { id = id, saveChangesError = true });
            }
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
