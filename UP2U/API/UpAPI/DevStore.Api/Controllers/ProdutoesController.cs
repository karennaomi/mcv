using DevStore.Domain;
using DevStore.Infra.DataContets;
using System;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;

namespace DevStore.Api.Controllers
{
    [EnableCors(origins:"*", headers:"*", methods:"*")]
    [RoutePrefix("api/v1/public")]
    public class ProdutoesController : ApiController
    {
        private DevStoreDataContents db = new DevStoreDataContents();

        //// GET: api/Produtoes
        //public IQueryable<Produto> GetProduts()
        //{
        //    return db.Produts.Include("Category");
        //}

        [Route("products")]
        public HttpResponseMessage GetProducts()
        {
             var result =  db.Produts.Include("Category");
             return Request.CreateResponse(HttpStatusCode.OK, result);

        }

        [Route("categories")]
        public HttpResponseMessage GetCategories()
        {
            var result = db.Categories.ToList();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [Route("categories/{categoryId}/products")]
        public HttpResponseMessage GetProductByCategory(int categoryId)
        {
            var result = db.Produts.Include("Category").Where(x => x.CategoryId == categoryId).ToList();
            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        [HttpPost]
        [Route("products")]
        public HttpResponseMessage PostProduct(Produto product)
        {
            if (product == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            try
            {
                db.Produts.Add(product);
                db.SaveChanges();
                var result = product;
                return Request.CreateResponse(HttpStatusCode.OK, result);

            }
            catch (Exception)
            {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError,"Falha ao incluir produto");
                }

        }
        [HttpPatch]
        [Route("products")]
        public HttpResponseMessage PatchProduct (Produto product)
        {
            if (product== null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try {
                db.Entry<Produto>(product).State = EntityState.Modified;
                db.SaveChanges();


                var result = product;
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Falha ao tentar atualizar produto");
            }
        }
        [HttpPut]
        [Route("products")]
        public HttpResponseMessage PutProduct(Produto product)
        {
            if (product == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try
            {
                db.Entry<Produto>(product).State = EntityState.Modified;
                db.SaveChanges();


                var result = product;
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Falha ao tentar atualizar produto");
            }
        }

        [HttpDelete]
        [Route("products")]
        public HttpResponseMessage DeleteProduct(int productId)
        {
            if(productId <= 0)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            try {

                db.Produts.Remove(db.Produts.Find(productId));
                db.SaveChanges();
                return Request.CreateResponse(HttpStatusCode.OK, "Produto excluido");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Problemas ao excluir produto");
            }

        }
        // GET: api/Produtoes/5
        [ResponseType(typeof(Produto))]
        public IHttpActionResult GetProduto(int id)
        {
            Produto produto = db.Produts.Find(id);
            if (produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }

        // PUT: api/Produtoes/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduto(int id, Produto produto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != produto.Id)
            {
                return BadRequest();
            }

            db.Entry(produto).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
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

        // POST: api/Produtoes
        [ResponseType(typeof(Produto))]
        public IHttpActionResult PostProduto(Produto produto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Produts.Add(produto);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = produto.Id }, produto);
        }

        // DELETE: api/Produtoes/5
        [ResponseType(typeof(Produto))]
        public IHttpActionResult DeleteProduto(int id)
        {
            Produto produto = db.Produts.Find(id);
            if (produto == null)
            {
                return NotFound();
            }

            db.Produts.Remove(produto);
            db.SaveChanges();

            return Ok(produto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProdutoExists(int id)
        {
            return db.Produts.Count(e => e.Id == id) > 0;
        }
    }
}