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

        /// <summary>
        /// Messages coming from the Bot. This is a List because the bot can send multiple messages at the same time
        /// </summary>
        public List<string> Messages { get; set; }

        public BotResponse(WitContext context)
        {
            this.Context = context;
            this.Messages = new List<string>();
        }
    }
}