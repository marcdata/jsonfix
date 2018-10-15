using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SampleFixHostPoc1.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class FauxInitiatorsController : Controller
    {


        public FauxInitiatorsController()
        {

        }


        // GET: api/Initiators
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "fauxinitiator1", "fauxinitiator2" };
        }

        // GET: api/Initiators/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "initiator";
        }

        // GET: api/Initiators/5
        [HttpGet("{id}/start")]
        public string StartInitiator(int id)
        {
            return $"initiator {id} started";
        }

        // GET: api/Initiators/5
        [HttpGet("{id}/stop")]
        public string StopInitiator(int id)
        {
            return $"initiator {id} stopped";
        }

        // GET: api/Initiators/5
        [HttpGet("{id}/fauxstart")]
        public string FauxStartInitiator(int id)
        {
            return $"initiator {id} started";
        }

        // GET: api/Initiators/5
        [HttpGet("{id}/fauxstop")]
        public string FauxStopInitiator(int id)
        {
            return $"initiator {id} stopped";
        }

        // POST: api/Initiators
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Initiators/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
