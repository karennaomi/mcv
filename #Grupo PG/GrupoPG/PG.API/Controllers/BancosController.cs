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
    public class BancosController : ApiController
    {
        private PGDataContents db = new PGDataContents();

        // GET: api/Bancos
        public IQueryable<Banco> GetBancoes()
        {
            return db.Bancos;
        }

        // GET: api/Bancos/5
        [ResponseType(typeof(Banco))]
        public IHttpActionResult GetBanco(int id)
        {
            Banco banco = db.Bancos.Find(id);
            if (banco == null)
            {
                return NotFound();
            }

            return Ok(banco);
        }

        // PUT: api/Bancos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBanco(int id, Banco banco)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != banco.Id)
            {
                return BadRequest();
            }

            db.Entry(banco).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BancoExists(id))
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

        // POST: api/Bancos
        [HttpPost]
        public HttpResponseMessage PostBanco(Banco banco)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.Bancos.Add(banco);
            db.Entry(banco).State = EntityState.Added;
            db.SaveChanges();

            var result = db.Bancos;
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // DELETE: api/Bancos/5
        [ResponseType(typeof(Banco))]
        public IHttpActionResult DeleteBanco(int id)
        {
            Banco banco = db.Bancos.Find(id);
            if (banco == null)
            {
                return NotFound();
            }

            db.Bancos.Remove(banco);
            db.Entry(banco).State = EntityState.Added;
            db.SaveChanges();

            return Ok(banco);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BancoExists(int id)
        {
            return db.Bancos.Count(e => e.Id == id) > 0;
        }
    }
}