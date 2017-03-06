using System.Collections.Generic;
using WitAi.Models;

namespace WitAi
{
    /// <summary>
    /// Represents a bot response
    /// </summary>
    public class BotResponse
    {


        /// <summary>
        /// Wit context. This is just a wrapper of a Dictionary
        /// </summary>
        public WitContext Context { get; set; }
        public string Message { get; set; }

        public BotResponse(WitContext context)
        {
            this.Context = context;
            this.Messages = new List<string>();
        }
    }
}