using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using EcheancierDotNet.DAL;
using EcheancierDotNet.Models;
using EcheancierDotNet.ViewModels;

namespace EcheancierDotNet.Controllers
{
    public class InvoicesController : ApiController
    {
        private PaymentContext db = new PaymentContext();

        // GET: api/Invoices
        public IEnumerable<InvoiceWrapper> GetInvoices()
        {            
            List<InvoiceWrapper> l_wrappersList = new List<InvoiceWrapper>();
            List<Invoice> l_invoicesList = db.Invoices.ToList();
            if (l_invoicesList == null)
                return (null);

            foreach (Invoice c in l_invoicesList)
            {
                l_wrappersList.Add(new InvoiceWrapper(c));
            }
            return l_wrappersList;
        }

        [Route("api/invoices/nonpaids/{p_currency_filter}")]
        [HttpGet]
        public IEnumerable<InvoiceWrapper> GetNonPaidInvoices(string p_currency_filter)
        {
            List<InvoiceWrapper> l_wrappersList = new List<InvoiceWrapper>();
            List<Invoice> l_invoicesList;

            if (p_currency_filter == "All")
            {
                l_invoicesList = db.Invoices.Where(i => i.Paid == false && i.DueDate.Year > 2017).OrderBy(i => i.DueDate).ToList();
            }
            else
            {
                l_invoicesList = db.Invoices.Where(i => i.Paid == false && i.Currency == p_currency_filter && i.DueDate.Year > 2017).OrderBy(i => i.DueDate).ToList();
            }
            
            if (l_invoicesList == null)
                return (null);

            foreach (Invoice c in l_invoicesList)
            {
                l_wrappersList.Add(new InvoiceWrapper(c));
            }
            return l_wrappersList;
        }

        [Route("api/invoices/tobepaids")]
        [HttpGet]
        public IEnumerable<InvoiceWrapper> GetToBePaidInvoices()
        {
            List<InvoiceWrapper> l_wrappersList = new List<InvoiceWrapper>();
            List<Invoice> l_invoicesList = db.Invoices.Where(i => (i.Paid == false) && (i.ToBePaid == true)).ToList();
            if (l_invoicesList == null)
                return (null);

            foreach (Invoice c in l_invoicesList)
            {
                l_wrappersList.Add(new InvoiceWrapper(c));
            }
            return l_wrappersList;
        }

        // GET: api/Invoices/5
        [ResponseType(typeof(Invoice))]
        public async Task<IHttpActionResult> GetInvoice(int id)
        {
            Invoice invoice = await db.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            return Ok(invoice);
        }

        // PUT: api/Invoices/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutInvoice(int id, Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != invoice.InvoiceID)
            {
                return BadRequest();
            }

            db.Entry(invoice).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Invoices
        [ResponseType(typeof(Invoice))]
        public async Task<IHttpActionResult> PostInvoice(Invoice invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Invoices.Add(invoice);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = invoice.InvoiceID }, invoice);
        }

        // DELETE: api/Invoices/5
        [ResponseType(typeof(Invoice))]
        public async Task<IHttpActionResult> DeleteInvoice(int id)
        {
            Invoice invoice = await db.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }

            db.Invoices.Remove(invoice);
            await db.SaveChangesAsync();

            return Ok(invoice);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool InvoiceExists(int id)
        {
            return db.Invoices.Count(e => e.InvoiceID == id) > 0;
        }
    }
}