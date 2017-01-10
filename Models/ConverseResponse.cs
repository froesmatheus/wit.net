using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Wit.Models
{
    public class ConverseResponse
    {
        public string Type { get; set; }
        public string Msg { get; set; }
        public List<string> QuickReplies { get; set; }
        public string Action { get; set; }
        public Dictionary<string, JArray> Entities { get; set; }
        public float Confidence { get; set; }
    }
}