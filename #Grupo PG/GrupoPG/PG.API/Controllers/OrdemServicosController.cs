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
    public class OrdemServicosController : ApiController
    {
        private PGDataContents db = new PGDataContents();
        
        [HttpGet]
        public HttpResponseMessage GetOrdemServico()
        {
            var result = db.OrdensServico;
            return Request.CreateResponse(HttpStatusCode.OK, result);
            db.Dispose();
        }
        


        // GET: api/OrdemServicos/5
        [ResponseType(typeof(OrdemServico))]
        public HttpResponseMessage GetOrdemServico(int id)
        {
            OrdemServico ordemServico = db.OrdensServico.Find(id);
            if (ordemServico == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, ordemServico);
        }

        // PUT: api/OrdemServicos/5
        [HttpPut]
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
        [HttpPost]
        public HttpResponseMessage PostOrdemServico(OrdemServico ordemServico)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            ordemServico.DataEntrega = DateTime.Now;
            ordemServico.DataFinalizacao = DateTime.Now;
            ordemServico.DtChegada = DateTime.Now;
            ordemServico.DtChegadaTransportadora = DateTime.Now;
            ordemServico.HoraEntrega = DateTime.Now;
            ordemServico.HrChegada = DateTime.Now;
            ordemServico.HrChegadaTransportadora = DateTime.Now;

            db.OrdensServico.Add(ordemServico);
            db.Entry(ordemServico).State = EntityState.Added;
            db.SaveChanges();
            var result = db.OrdensServico;
            return Request.CreateResponse(HttpStatusCode.OK, result) ;
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