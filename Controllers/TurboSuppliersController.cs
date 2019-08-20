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
using EcheancierDotNet.Models.Uploaders;

namespace EcheancierDotNet.Controllers
{
    public class TurboSuppliersController : Controller
    {
        private PaymentContext db = new PaymentContext();
        private string m_message = "";
        private string m_alert = "";

        // GET: TurboSuppliers
        public ActionResult Index()
        {
            if (m_message != "")
            {
                ViewBag.message = m_message;
            }

            if (m_alert != "")
            {
                ViewBag.Alert = m_alert;
            }

            IEnumerable<TurboSupplier> l_turboSuppliers = db.TurboSupplier.ToList();

            return View(l_turboSuppliers);
        }

        // GET: TurboSuppliers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TurboSupplier turboSupplier = db.TurboSupplier.Find(id);
            if (turboSupplier == null)
            {
                return HttpNotFound();
            }
            return View(turboSupplier);
        }


        // This action handles the form POST and the upload
        [HttpPost]
        public ActionResult CreateOrUpdateFromCsv(HttpPostedFileBase file, SupplierPaymentArea p_paymentArea)
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

                var l_suppliers = db.TurboSupplier.ToList();
                TurboSupplierUploader l_uploader = new TurboSupplierUploader(l_suppliers);

                bool upload_result = l_uploader.UpdateOrCreateTurboSupplierFromCSV(p_path, p_paymentArea);
                if (upload_result == true)
                {
                    try
                    {
                        foreach (TurboSupplier l_supplier in l_uploader.m_suppliers_to_create)
                        {
                            if (ModelState.IsValid)
                            {
                                db.TurboSupplier.Add(l_supplier);
                                db.SaveChanges();
                            }
                        }
                        m_message = "Creation success, number of Turbo suppliers created: " + l_uploader.m_suppliers_to_create.Count.ToString();

                        foreach (TurboSupplier l_supplier in l_uploader.m_suppliers_to_update)
                        {
                            if (ModelState.IsValid)
                            {
                                db.Entry(l_supplier).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                        }
                        m_message = m_message +  " - Update success, number of Turbo suppliers updated: " + l_uploader.m_suppliers_to_update.Count.ToString();
                    }
                    catch (DataException /* dex */)
                    {
                        //Log the error (uncomment dex variable name and add a line here to write a log.
                        m_alert = "Unable to save changes. Try again, and if the problem persists see your system administrator.";
                        ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                    }
                }
            }
            else
            {
                m_alert = "No file selected";
            }
            // send back to index page with message : success ; error ; ok duplicates 
            return RedirectToAction("Index");
        }


        // GET: TurboSuppliers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TurboSuppliers/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TurboSupplierID,PaymentArea,TYPE,BENEF_CODE,BENEF_RS,SIREN,NOM,BENEF_AD1,BENEF_CP,BENEF_VILLE,BENEF_MAIL,BENEF_CODEPAYS,DOB_IBAN,BQE_BIC,BQE_NOM,BQE_CODEPAYS,GROUPE,MONTANT,DEVISE,SOCIETE_BENEF,DOB_TYPE")] TurboSupplier turboSupplier)
        {
            if (ModelState.IsValid)
            {
                db.TurboSupplier.Add(turboSupplier);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(turboSupplier);
        }

        // GET: TurboSuppliers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TurboSupplier turboSupplier = db.TurboSupplier.Find(id);
            if (turboSupplier == null)
            {
                return HttpNotFound();
            }
            return View(turboSupplier);
        }

        // POST: TurboSuppliers/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "TurboSupplierID,PaymentArea,TYPE,BENEF_CODE,BENEF_RS,SIREN,NOM,BENEF_AD1,BENEF_CP,BENEF_VILLE,BENEF_MAIL,BENEF_CODEPAYS,DOB_IBAN,BQE_BIC,BQE_NOM,BQE_CODEPAYS,GROUPE,MONTANT,DEVISE,SOCIETE_BENEF,DOB_TYPE")] TurboSupplier turboSupplier)
        {
            if (ModelState.IsValid)
            {
                db.Entry(turboSupplier).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(turboSupplier);
        }

        // GET: TurboSuppliers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TurboSupplier turboSupplier = db.TurboSupplier.Find(id);
            if (turboSupplier == null)
            {
                return HttpNotFound();
            }
            return View(turboSupplier);
        }

        // POST: TurboSuppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TurboSupplier turboSupplier = db.TurboSupplier.Find(id);
            db.TurboSupplier.Remove(turboSupplier);
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
