using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wit.Models
{
    public class ConverseRequest
    {
        public string SessionId { get; set; }
        [JsonProperty("q")]
        public string Message { get; set; }
        public WitContext Context { get; set; }
        public Dictionary<string, JArray> Entities { get; set; }

        public ConverseRequest()
        {

        }

        public ConverseRequest(string sessionId, string message, WitContext context)
        {
            this.SessionId = sessionId;
            this.Message = message;
            this.Context = context; 
        }
    }
}