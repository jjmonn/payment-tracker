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

namespace EcheancierDotNet.Controllers
{
    public class FlowsController : ApiController
    {
        private PaymentContext db = new PaymentContext();

        // GET: api/Flows
        public IQueryable<Flow> GetFlow()
        {
            return db.Flow;
        }

        // GET: api/Flows/5
        [ResponseType(typeof(Flow))]
        public async Task<IHttpActionResult> GetFlow(int id)
        {
            Flow flow = await db.Flow.FindAsync(id);
            if (flow == null)
            {
                return NotFound();
            }

            return Ok(flow);
        }

        // PUT: api/Flows/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutFlow(int id, Flow flow)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != flow.ID)
            {
                return BadRequest();
            }

            db.Entry(flow).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FlowExists(id))
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

        // POST: api/Flows
        [ResponseType(typeof(Flow))]
        public async Task<IHttpActionResult> PostFlow(Flow flow)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Flow.Add(flow);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = flow.ID }, flow);
        }

        // DELETE: api/Flows/5
        [ResponseType(typeof(Flow))]
        public async Task<IHttpActionResult> DeleteFlow(int id)
        {
            Flow flow = await db.Flow.FindAsync(id);
            if (flow == null)
            {
                return NotFound();
            }

            db.Flow.Remove(flow);
            await db.SaveChangesAsync();

            return Ok(flow);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool FlowExists(int id)
        {
            return db.Flow.Count(e => e.ID == id) > 0;
        }
    }
}