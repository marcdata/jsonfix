using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using SampleFixHostPoc1.Domain.Services;



namespace SampleFixHostPoc1.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class InitiatorsController : Controller
    {
        IFixInitiatorHostService _fixInitiatorHostService;

        public InitiatorsController(IFixInitiatorHostService fixInitiatorHostService)
        {
            _fixInitiatorHostService = fixInitiatorHostService ?? throw new ArgumentNullException(nameof(fixInitiatorHostService));
            // _fixInitiatorHostService.InitializeSelf();
        }

        // GET: api/Initiators
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var sessionNames = new List<string>();
            var initiators = _fixInitiatorHostService.GetInitiators();
            initiators.ToList().ForEach(x => sessionNames.Add(
                x.GetSessionShortname()));
            return sessionNames;
        }

        // GET: api/Initiators/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            var initiator = _fixInitiatorHostService.GetInitiators().Skip(id).FirstOrDefault();
            if (initiator == null)
            {
                return "initiator not found";
            }
            return $"initiator, {initiator.GetSessionShortname()}";
        }

        // GET: api/Initiators/5
        [HttpGet("{id}/start")]
        public string StartInitiator(int id)
        {
            var initiator = _fixInitiatorHostService.GetInitiators().Skip(id).FirstOrDefault();
            if (initiator == null)
            {
                return "initiator not found";
            }
            initiator.Start();
            return $"initiator {id} started";
        }

        // GET: api/Initiators/5
        [HttpGet("{id}/stop")]
        public string StopInitiator(int id)
        {
            var initiator = _fixInitiatorHostService.GetInitiators().Skip(id).FirstOrDefault();
            if(initiator == null)
            {
                return "initiator not found";
            }
            initiator.Stop();
            return $"initiator {id} stopped";
        }

        // GET: api/Initiators/5
        [HttpGet("{id}/status")]
        public string InitiatorStatus(int id)
        {
            // do zero-based indexing
            var initiator = _fixInitiatorHostService.GetInitiators().Skip(id).FirstOrDefault();

            if(initiator == null)
            {
                return "initiator not found";
            }

            return $"initiator {id} status: {initiator.Status()}";
        }

        // GET: api/Initiators/5
        [HttpGet("{id}/getSessionInfo")]
        public string InitiatorGetSession(int id)
        {
            // goal here is get access to session for to send messages on.

            var initiator = _fixInitiatorHostService.GetInitiators().Skip(id).FirstOrDefault();

            if (initiator == null)
            {
                return "initiator not found";
            }

            var session = initiator; // get session from underlying fix component
            return "session info not found (todo: add more code.)";

            return $"initiator {id} status: {initiator.Status()}";
        }


        // GET: api/Initiators/5
        [HttpGet("{id}/testRequestWithMessage")]
        public string InitiatorTestRequestMessage(int id, [FromQuery]string msg)
        {
            msg = msg.Trim().Substring(0, Math.Min(msg.Length, 20));
            if(msg == "") { msg = "TestMessage."; }

            // do zero-based indexing
            var initiator = _fixInitiatorHostService.GetInitiators().Skip(id).FirstOrDefault();

            if (initiator == null)
            {
                return "initiator not found";
            }

            var sessionIdFromInitiator = initiator.GetSessionShortname(); // careful

            var testRequest = new QuickFix.FIX42.TestRequest();
            // testRequest.SetField(new QuickFix.Fields.Text(msg));
            testRequest.SetField(new QuickFix.Fields.TestReqID(msg));
            var sessionId = new QuickFix.SessionID("FIX.4.2", "MJV_CLIENT1", "SIMPLE"); // todo: grab from different session tracking resource // from initiator probably.

            // about to Send A Message!
            QuickFix.Session.SendToTarget(testRequest, sessionId);

            return $"Partial sent message: {testRequest.ToString()}";
        }

        // ok -- now that some client generated communication is started,
        // next up is generate 1) from rawfixmessage and send that (not a subclass of messagetype
        // then other ways to send thru from jsonpayload and postman

            // then modifying the FixApp logic to handle more / better logging on the webapi/rest side. 

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
