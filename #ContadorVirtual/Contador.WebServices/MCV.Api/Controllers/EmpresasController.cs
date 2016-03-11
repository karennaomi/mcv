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
using MCV.Domain;
using MCV.Infra.DataContents;
using System.Web.Http.Cors;
using System.Xml;
using MCV.Infra;
using System.Web.Mvc;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Helpers;
using System.Web.Http.Results;

namespace MCV.Api.Controllers
{

    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [System.Web.Http.RoutePrefix("api/v1/public")]
    public class EmpresasController : ApiController
    {
        private MCVDataContents db = new MCVDataContents();
        private CookieContainer _cookies;
        private readonly string urlBaseReceitaFederal = "http://www.receita.fazenda.gov.br/pessoajuridica/cnpj/cnpjreva/";
        private readonly string paginaValidacao = "valida.asp";
        private readonly string paginaPrincipal = "cnpjreva_solicitacao2.asp";
        private readonly string paginaCaptcha= "captcha/gerarCaptcha.asp";


        

        // PUT: api/Empresas/5
        
        [System.Web.Http.Route("api/consultaCNPJ")]
        public HttpResponseMessage consultaCNPJ(string cnpj)
        {
            if (cnpj == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            try
            {
                GetCaptcha();
                return Request.CreateResponse(HttpStatusCode.Accepted, "");

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Falha ao consultar cnpj");
            }

            
        }

        public JsonResult<string> GetCaptcha()
        {

            var htmlResult = string.Empty;

            using (var wc = new Infra.CookieAwareWebClient(_cookies))
            {
                wc.Headers[HttpRequestHeader.UserAgent] = "Mozilla/4.0 (compatible; Synapse)";
                wc.Headers[HttpRequestHeader.KeepAlive] = "300";
                htmlResult = wc.DownloadString(urlBaseReceitaFederal + paginaPrincipal);
            }

            if (htmlResult.Length > 0)
            {
                var wc2 = new CookieAwareWebClient(_cookies);
                wc2.Headers[HttpRequestHeader.UserAgent] = "Mozilla/4.0 (compatible; Synapse)";
                wc2.Headers[HttpRequestHeader.KeepAlive] = "300";
                byte[] data = wc2.DownloadData(urlBaseReceitaFederal + paginaCaptcha);

                HttpContext.Current.Session["cookies"] = _cookies;

                var retorno = "data:image/jpeg;base64," + Convert.ToBase64String(data, 0, data.Length);
                JsonSerializerSettings jsSettings = new JsonSerializerSettings();
                jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                var converted = JsonConvert.SerializeObject(retorno, null, jsSettings);

                //return Content(converted, "application/json");

               return Json(retorno, jsSettings);
               

            }

            return null;

        }

        public void ConsultarDados(string cnpj, string captcha)
        {
            var msg = string.Empty;
            var resp = ObterDados(cnpj, captcha);

            if (resp.Contains("Verifique se o mesmo foi digitado corretamente"))
                msg = "O número do CNPJ não foi digitado corretamente";

            if (resp.Contains("Erro na Consulta"))
                msg += "Os caracteres não conferem com a imagem";

            JsonSerializerSettings jsSettings = new JsonSerializerSettings();
            jsSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            

            //return Json(
            //    new
            //    {
            //        erro = msg,
            //        dados = resp.Length > 0 ? FormatarDados.MontarObjEmpresa(cnpj, resp) : null
            //    }, serializerSettings: JsonRequestBehavior.DenyGet);
        }

        private string ObterDados(string aCNPJ, string aCaptcha)
        {
            _cookies = (CookieContainer)HttpContext.Current.Session["cookies"];

            var request = (HttpWebRequest)WebRequest.Create(urlBaseReceitaFederal + paginaValidacao);
            request.ProtocolVersion = HttpVersion.Version10;
            request.CookieContainer = _cookies;
            request.Method = "POST";

            var postData = string.Empty;
            postData += "origem=comprovante&";
            postData += "cnpj=" + new Regex(@"[^\d]").Replace(aCNPJ, string.Empty) + "&";
            postData += "txtTexto_captcha_serpro_gov_br=" + aCaptcha + "&";
            postData += "submit1=Consultar&";
            postData += "search_type=cnpj";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            var dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();

            var stHtml = new StreamReader(request.GetResponse().GetResponseStream(), Encoding.GetEncoding("ISO-8859-1"));
            return stHtml.ReadToEnd();
        }

        // GET: api/Empresas
        public IQueryable<Empresas> GetEmpresas()
        {
            return db.Empresas;
            
        }
        
       
        // GET: api/Empresas/5
        [ResponseType(typeof(Empresas))]
        public IHttpActionResult GetEmpresas(int id)
        {
            Empresas empresas = db.Empresas.Find(id);
            if (empresas == null)
            {
                return NotFound();
            }

            return Ok(empresas);
        }

        // PUT: api/Empresas/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutEmpresas(int id, Empresas empresas)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != empresas.Id)
            {
                return BadRequest();
            }

            db.Entry(empresas).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpresasExists(id))
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

  
        // POST: api/Empresas
        [System.Web.Http.HttpPost]
        public HttpResponseMessage PostEmpresas(Empresas empresas)
        {
            if (empresas == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            try 
            {
                
                db.Empresas.Add(empresas);
                db.SaveChanges();
                var result = empresas;
                ConsultaPost.ConsultaMatrizSoapClient consulta = new ConsultaPost.ConsultaMatrizSoapClient();

                var retorno =  consulta.ConsultaMatriz(result.CNPJ.ToString());
                return Request.CreateResponse(HttpStatusCode.OK, result);
             

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Falha ao incluir Empresa");
            }
        }

        // POST: api/Empresas
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("api/cancelamento")]
        public HttpResponseMessage Cancelamento(Empresas empresas)
        {
            
       
            if (empresas == null)
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            try
            {
                SP nota = new SP();
                XmlDocument doc = new XmlDocument();

                //var retornoNata = nota.Cancelamento(empresas.CNPJ, "");
                //doc.InnerText = retornoNata;
                

                db.Empresas.Add(empresas);
                db.SaveChanges();
                var result = empresas;
                ConsultaPost.ConsultaMatrizSoapClient consulta = new ConsultaPost.ConsultaMatrizSoapClient();

                var retorno = consulta.ConsultaMatriz(result.CNPJ.ToString());
                return Request.CreateResponse(HttpStatusCode.OK, result);


            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, "Falha ao incluir Empresa");
            }
        }
        // DELETE: api/Empresas/5
        [ResponseType(typeof(Empresas))]
        public IHttpActionResult DeleteEmpresas(int id)
        {
            Empresas empresas = db.Empresas.Find(id);
            if (empresas == null)
            {
                return NotFound();
            }

            db.Empresas.Remove(empresas);
            db.SaveChanges();

            return Ok(empresas);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmpresasExists(int id)
        {
            return db.Empresas.Count(e => e.Id == id) > 0;
        }
    }
}