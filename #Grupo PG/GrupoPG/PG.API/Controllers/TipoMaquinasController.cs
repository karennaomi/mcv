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
    public class TipoMaquinasController : ApiController
    {
        private PGDataContents db = new PGDataContents();

        // GET: api/TipoMaquinas
        public IQueryable<TipoMaquina> GetTipoMaquinas()
        {
            return db.TpMaquina;
        }

        // GET: api/TipoMaquinas/5
        [ResponseType(typeof(TipoMaquina))]
        public IHttpActionResult GetTipoMaquina(int id)
        {
            TipoMaquina tipoMaquina = db.TpMaquina.Find(id);
            if (tipoMaquina == null)
            {
                return NotFound();
            }

            return Ok(tipoMaquina);
        }

        // PUT: api/TipoMaquinas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTipoMaquina(int id, TipoMaquina tipoMaquina)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoMaquina.Id)
            {
                return BadRequest();
            }

            db.Entry(tipoMaquina).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoMaquinaExists(id))
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

        // POST: api/TipoMaquinas
        [ResponseType(typeof(TipoMaquina))]
        public IHttpActionResult PostTipoMaquina(TipoMaquina tipoMaquina)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.TpMaquina.Add(tipoMaquina);
            db.Entry(tipoMaquina).State = EntityState.Added;
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tipoMaquina.Id }, tipoMaquina);
        }

        // DELETE: api/TipoMaquinas/5
        [ResponseType(typeof(TipoMaquina))]
        public IHttpActionResult DeleteTipoMaquina(int id)
        {
            TipoMaquina tipoMaquina = db.TpMaquina.Find(id);
            if (tipoMaquina == null)
            {
                return NotFound();
            }

            db.TpMaquina.Remove(tipoMaquina);
            db.SaveChanges();

            return Ok(tipoMaquina);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoMaquinaExists(int id)
        {
            return db.TpMaquina.Count(e => e.Id == id) > 0;
        }
    }
}