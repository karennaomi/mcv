using PG.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using PG.Infra;
using PG.Infra.DataContents;

namespace API
{
    /// <summary>
    /// Summary description for grupopgapi
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class grupopgapi : System.Web.Services.WebService
    {
//        private PGDataContents db = new PGDataContents();

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [WebMethod]
        public static void  InserirBanco(string NomeBanco)
        {
             PGDataContents db = new PGDataContents();
             var banco = new Banco();
             banco.NomeBanco = NomeBanco;
            db.Bancos.Add(banco);

        }


    }
}
