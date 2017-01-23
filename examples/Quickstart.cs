using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WitAi;
using WitAi.Models;

namespace WitAi.Examples
{
    // Quickstart example
    // See https://wit.ai/ar7hur/Quickstart
    class Quickstart
    {
        static void Main(string[] args)
        {
            var actions = new WitActions();
            actions["send"] = Send;
            actions["getForecast"] = GetForecast;


            Wit wit = new Wit(accessToken: "<SERVER_ACCESS_TOKEN>", actions: actions);
            wit.Converse("session-id-01", "Hi!", new WitContext());
        }

        private static WitContext GetForecast(ConverseRequest request, ConverseResponse response)
        {
            var context = request.Context;
            var entities = request.Entities;

            var loc = FirstEntityValue(entities, "location");
            if (loc != null)
            {
                context["forecast"] = "sunny";
                if (context.ContainsKey("missingLocation")) {
                    context.Remove("missingLocation");
                }
            } else
            {
                context["missingLocation"] = true;
                if (context.ContainsKey("forecast"))
                {
                    context.Remove("forecast");
                }
            }
            return context;
        }

        private static WitContext Send(ConverseRequest request, ConverseResponse response)
        {
            Console.WriteLine(response.Msg);
            return request.Context;
        }




        private static string FirstEntityValue(Dictionary<string, dynamic> entities, string entity)
        {
            if (!entities.ContainsKey(entity))
            {
                return null;
            }
            var value = entities[entity][0]["value"];

            if (value == null)
            {
                return null;
            }

            if (value is Dictionary<string, dynamic>)
            {
                return value["value"];
            }
            else
            {
                return value;
            }
        }
    }
}
