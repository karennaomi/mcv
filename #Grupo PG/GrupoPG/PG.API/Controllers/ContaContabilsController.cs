using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PG.Domain;
using PG.Infra.DataContents;

namespace PG.API.Controllers
{
    public class ContaContabilsController : ApiController
    {
        private PGDataContents db = new PGDataContents();

        // GET: api/ContaContabils
        public HttpResponseMessage GetContaContabils()
        {
            var contaContabil = db.ContasContabil;
            return Request.CreateResponse(HttpStatusCode.OK, contaContabil); ;
        }

        // GET: api/ContaContabils/5
        public HttpResponseMessage GetContaContabil(int id)
        {
            ContaContabil contaContabil = db.ContasContabil.Find(id);
            if (contaContabil == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return Request.CreateResponse(HttpStatusCode.OK,contaContabil);
        }

        // PUT: api/ContaContabils/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutContaContabil(int id, ContaContabil contaContabil)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contaContabil.Id)
            {
                return BadRequest();
            }

            db.Entry(contaContabil).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContaContabilExists(id))
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

        // POST: api/ContaContabils
        [ResponseType(typeof(ContaContabil))]
        public IHttpActionResult PostContaContabil(ContaContabil contaContabil)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ContasContabil.Add(contaContabil);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = contaContabil.Id }, contaContabil);
        }

        // DELETE: api/ContaContabils/5
        [ResponseType(typeof(ContaContabil))]
        public IHttpActionResult DeleteContaContabil(int id)
        {
            ContaContabil contaContabil = db.ContasContabil.Find(id);
            if (contaContabil == null)
            {
                return NotFound();
            }

            db.ContasContabil.Remove(contaContabil);
            db.SaveChanges();

            return Ok(contaContabil);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContaContabilExists(int id)
        {
            return db.ContasContabil.Count(e => e.Id == id) > 0;
        }
    }
}