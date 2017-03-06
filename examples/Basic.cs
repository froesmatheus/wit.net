using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WitAi;
using WitAi.Models;

namespace WitAi.Examples
{
    class Basic
    {
        static void Main(string[] args)
        {
            var actions = new WitActions();
            actions["send"] = Send;


            Wit wit = new Wit(accessToken: "<SERVER_ACCESS_TOKEN>", actions: actions);
            wit.Converse("session-id-01", "Hi!", new WitContext());
        }

        private static WitContext Send(ConverseRequest request, ConverseResponse response)
        {
            Console.WriteLine(response.Msg);
            return request.Context;
        }
    }
}
