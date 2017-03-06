using System.Collections.Generic;
using WitAi.Models;

namespace WitAi
{
    public class BotResponse
    {
        public WitContext Context { get; set; }
        public List<string> Messages { get; set; }

        public BotResponse(WitContext context)
        {
            this.Context = context;
            this.Messages = new List<string>();
        }
    }
}