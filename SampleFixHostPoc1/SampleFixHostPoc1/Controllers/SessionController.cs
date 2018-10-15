using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SampleFixHostPoc1.Controllers
{
    [Produces("application/json")]
    [Route("api/session")]
    public class SessionController : Controller
    {

        private List<string> _fakeSessions = new List<string>() { "session1, session2" };


        public SessionController()
        {

        }

        // GET: api/Session
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "session1", "session2" };
        //}

        //// GET: api/Session/5
        //[HttpGet("{id}", Name = "Get")]
        //public string Get(int id)
        //{
        //    return id.ToString();
        //}

        // GET: api/Session/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id, [FromQuery]string value)
        {
            var msg = id.ToString() + $" set to status {value}";
            return msg;
        }

        // GET: api/Session/5
        [HttpGet("{id}/pseudoPut", Name = "pseudoPut")]
        public string GetPseudoPut(int id, [FromQuery]string value)
        {
            var msg = id.ToString() + $" set to status {value}";
            return msg;
        }

        // GET: api/Session/5
        [HttpGet("{id}/pseudoSessionPut", Name = "pseudoSessionPut")]
        public string GetPseudoSessionPutt(int id, [FromQuery]string value)
        {
            var msg = id.ToString() + $" set to status (started) {value}";
            return msg;
        }

        // POST: api/Session
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Session/5
        [HttpPut("{id}")]
        public void Put(int id, [FromQuery]string value)
        {

        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
