using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace GrupoPG.WEB.Controllers
{
    public class OrdemServicesController : ApiController
    {
        // GET: api/OrdemServices
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/OrdemServices/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/OrdemServices
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/OrdemServices/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/OrdemServices/5
        public void Delete(int id)
        {
        }
    }
}
