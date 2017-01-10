using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MagisterBotApi.Models
{
    public class MessageRequest
    {
        [JsonProperty("q")]
        public string Query { get; set; }
        [JsonProperty("msg_id")]
        public string MsgId { get; set; }
        [JsonProperty("thread_id")]
        public int ThreadId { get; set; }

        public MessageRequest(string query, string msgId = "", int threadId = 0) 
        {
            this.Query = query;
            this.MsgId = msgId;
            this.ThreadId = threadId;
        }
    }
}