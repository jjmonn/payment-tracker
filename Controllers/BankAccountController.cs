using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using EcheancierDotNet.DAL;
using EcheancierDotNet.Models;

namespace EcheancierDotNet.Controllers
{
    public class BankAccountController : Controller
    {
        private PaymentContext db = new PaymentContext();

        // GET: BankAccounts
        public async Task<ActionResult> Index()
        {
            return View(await db.BankAccount.ToListAsync());
        }

        // GET: BankAccounts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = await db.BankAccount.FindAsync(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // GET: BankAccounts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: BankAccounts/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ID,Name,BankCode,AgencyCode,AccountNumber,RIBKey,BIC,IBAN,Currency,BankAddress,BankCountry")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                db.BankAccount.Add(bankAccount);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(bankAccount);
        }

        // GET: BankAccounts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = await db.BankAccount.FindAsync(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // POST: BankAccounts/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ID,Name,BankCode,AgencyCode,AccountNumber,RIBKey,BIC,IBAN,Currency,BankAddress,BankCountry")] BankAccount bankAccount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bankAccount).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(bankAccount);
        }

        // GET: BankAccounts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BankAccount bankAccount = await db.BankAccount.FindAsync(id);
            if (bankAccount == null)
            {
                return HttpNotFound();
            }
            return View(bankAccount);
        }

        // POST: BankAccounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            BankAccount bankAccount = await db.BankAccount.FindAsync(id);
            db.BankAccount.Remove(bankAccount);
            await db.SaveChangesAsync();
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
