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
    public class SuppliersController : ApiController
    {
        private PaymentContext db = new PaymentContext();

        // GET: api/Suppliers
        public IEnumerable<SupplierWrapper> GetSuppliers()
        {
            List<SupplierWrapper> l_wrappersList = new List<SupplierWrapper>();
            List<Supplier> l_suppliersList = db.Suppliers.ToList();
            if (l_suppliersList == null)
                return (null);

            foreach (Supplier c in l_suppliersList)
            {
                l_wrappersList.Add(new SupplierWrapper(c));
            }
            return l_wrappersList;
        }


        [Route("api/suppliers/tobepaid")]
        [HttpGet]
        public IEnumerable<SupplierWrapper> GetSuppliersToBePaid()
        {
            List<SupplierWrapper> l_wrappersList = new List<SupplierWrapper>();
            List<Supplier> l_suppliersList = db.Suppliers.ToList();
            if (l_suppliersList == null)
                return (null);

            foreach (Supplier c in l_suppliersList)
            {
                SupplierWrapper l_supplierWrapper = new SupplierWrapper(c, true);
                if (l_supplierWrapper.Invoices.Count > 0)
                {
                    l_wrappersList.Add(l_supplierWrapper);
                }
            }
            return l_wrappersList;
        }


        // GET: api/Suppliers/5
        [ResponseType(typeof(Supplier))]
        public async Task<IHttpActionResult> GetSupplier(int id)
        {
            Supplier supplier = await db.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            return Ok(supplier);
        }

        // PUT: api/Suppliers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutSupplier(int id, Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != supplier.ID)
            {
                return BadRequest();
            }

            db.Entry(supplier).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SupplierExists(id))
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

        // POST: api/Suppliers
        [ResponseType(typeof(Supplier))]
        public async Task<IHttpActionResult> PostSupplier(Supplier supplier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Suppliers.Add(supplier);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = supplier.ID }, supplier);
        }

        // DELETE: api/Suppliers/5
        [ResponseType(typeof(Supplier))]
        public async Task<IHttpActionResult> DeleteSupplier(int id)
        {
            Supplier supplier = await db.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                return NotFound();
            }

            db.Suppliers.Remove(supplier);
            await db.SaveChangesAsync();

            return Ok(supplier);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SupplierExists(int id)
        {
            return db.Suppliers.Count(e => e.ID == id) > 0;
        }
    }
}