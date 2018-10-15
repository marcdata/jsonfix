using System;
using System.Collections.Generic;
using System.Text;

namespace FixServiceLib.Contracts
{
    public class JsonFixMessage
    {
        public string localPseudoHeader { get; set; }
        public string rawFixPayload { get; set; }
    }

    // for this, store diff header information to support message_as_type reconstruction
    public class JsonFixMessageReceived : JsonFixMessage
    {
        // public string localPseudoHeader { get; set; }
        public string messageTypeCode35 { get; set; }
        // public string rawFixPayload { get; set; }
    }
}
