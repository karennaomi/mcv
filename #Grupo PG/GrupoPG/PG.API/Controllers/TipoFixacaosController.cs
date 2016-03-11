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
    public class TipoFixacaosController : ApiController
    {
        private PGDataContents db = new PGDataContents();

        // GET: api/TipoFixacaos
        [HttpGet]
        public HttpResponseMessage GetTipoFixacaos()
        {
            return Request.CreateResponse(HttpStatusCode.OK, db.TpFixacao);
            
        }

        // GET: api/TipoFixacaos/5
        [HttpGet]
        public HttpResponseMessage GetTipoFixacao(int id)
        {
            TipoFixacao tipoFixacao = db.TpFixacao.Find(id);
            if (tipoFixacao == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, tipoFixacao);
        }

        // PUT: api/TipoFixacaos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTipoFixacao(int id, TipoFixacao tipoFixacao)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tipoFixacao.Id)
            {
                return BadRequest();
            }

            db.Entry(tipoFixacao).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoFixacaoExists(id))
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

        // POST: api/TipoFixacaos
        [ResponseType(typeof(TipoFixacao))]
        public HttpResponseMessage PostTipoFixacao(TipoFixacao tipoFixacao)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            db.TpFixacao.Add(tipoFixacao);
            db.SaveChanges();
            var result = db.TpFixacao;

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        // DELETE: api/TipoFixacaos/5
        [ResponseType(typeof(TipoFixacao))]
        public IHttpActionResult DeleteTipoFixacao(int id)
        {
            TipoFixacao tipoFixacao = db.TpFixacao.Find(id);
            if (tipoFixacao == null)
            {
                return NotFound();
            }

            db.TpFixacao.Remove(tipoFixacao);
            db.SaveChanges();

            return Ok(tipoFixacao);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TipoFixacaoExists(int id)
        {
            return db.TpFixacao.Count(e => e.Id == id) > 0;
        }
    }
}