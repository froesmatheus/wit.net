using WitAi.Models;

namespace WitAi
{
    public class BotResponse
    {
        public WitContext Context { get; set; }
        public string Message { get; set; }

        public BotResponse(WitContext context)
        {
            this.Context = context;
        }
    }
}