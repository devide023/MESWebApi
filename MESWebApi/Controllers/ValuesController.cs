using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using log4net;
using MESWebApi.DB;
using Dapper;
using System.Threading.Tasks;
using System.Web.Http.Results;
using MESWebApi;
using MESWebApi.Util;

namespace MESWebApi.Controllers
{
    [CheckLogin]
    public class ValuesController : ApiController
    {
        private ILog log;
        public ValuesController()
        {
           this.log = LogManager.GetLogger(this.GetType());
        }
        // GET api/values
        public async Task<IHttpActionResult> Get()
        {
            using (var db = new OraDBHelper())
            {
               var d = db.Conn.QueryAsync("SELECT btj,sblx,xh,bzbb FROM base_btbz where rownum<10");
                return Json(new {list=await d});
            }
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
