using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WitAi.Models
{
    public class MessageResponse
    {
        [JsonProperty("msg_id")]
        public string MsgId { get; set; }
        [JsonProperty("_text")]
        public string Text { get; set; }
        public Dictionary<string, JArray> Entities { get; set; }
    }
}