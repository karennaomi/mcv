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

namespace GrupoPG.WEB.Controllers
{
    public class OrdemServicosController : ApiController
    {
        private PGDataContents db = new PGDataContents();

        // GET: api/OrdemServicos
        public IQueryable<OrdemServico> GetOrdemServicoes()
        {
            return db.OrdensServico;
            
        }

        // GET: api/OrdemServicos/5
        [ResponseType(typeof(OrdemServico))]
        public IHttpActionResult GetOrdemServico(int id)
        {
            OrdemServico ordemServico = db.OrdensServico.Find(id);
            if (ordemServico == null)
            {
                return NotFound();
            }

            return Ok(ordemServico);
        }

        // PUT: api/OrdemServicos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrdemServico(int id, OrdemServico ordemServico)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != ordemServico.Id)
            {
                return BadRequest();
            }

            db.Entry(ordemServico).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrdemServicoExists(id))
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

        // POST: api/OrdemServicos
        [ResponseType(typeof(OrdemServico))]
        public IHttpActionResult PostOrdemServico(OrdemServico ordemServico)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OrdensServico.Add(ordemServico);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = ordemServico.Id }, ordemServico);
        }

        // DELETE: api/OrdemServicos/5
        [ResponseType(typeof(OrdemServico))]
        public IHttpActionResult DeleteOrdemServico(int id)
        {
            OrdemServico ordemServico = db.OrdensServico.Find(id);
            if (ordemServico == null)
            {
                return NotFound();
            }

            db.OrdensServico.Remove(ordemServico);
            db.SaveChanges();

            return Ok(ordemServico);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrdemServicoExists(int id)
        {
            return db.OrdensServico.Count(e => e.Id == id) > 0;
        }
    }
}