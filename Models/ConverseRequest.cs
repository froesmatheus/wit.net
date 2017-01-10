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
        public int SessionId { get; set; }
        [JsonProperty("q")]
        public string Message { get; set; }
        public Dictionary<string, dynamic> Context { get; set; }
        public Dictionary<string, JArray> Entities { get; set; }

        public ConverseRequest()
        {

        }

        public ConverseRequest(int sessionId, string message, Dictionary<string, dynamic> context)
        {
            this.SessionId = sessionId;
            this.Message = message;
            this.Context = context; 
        }
    }
}