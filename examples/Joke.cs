using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WitAi;
using WitAi.Models;

namespace WitAi.Examples
{

    // Joke example
    // See https://wit.ai/patapizza/example-joke
    class Joke
    {
        static Dictionary<string, List<string>> allJokes = new Dictionary<string, List<string>>()
        {
            { "chuck", new List<string>() {
                "Chuck Norris counted to infinity - twice.",
                "Death once had a near-Chuck Norris experience."}
            },

            { "tech", new List<string>() {
               "Did you hear about the two antennas that got married? The ceremony was long and boring, but the reception was great!",
               "Why do geeks mistake Halloween and Christmas? Because Oct 31 === Dec 25." }
            },

            { "default", new List<string>() {
                "Why was the Math book sad? Because it had so many problems." }
            }
        };

        static void Main(string[] args)
        {
            var actions = new WitActions();
            actions["send"] = Send;
            actions["merge"] = Merge;
            actions["select-joke"] = SelectJoke;

            Wit wit = new Wit(accessToken: "<SERVER_ACCESS_TOKEN>", actions: actions);
            wit.Converse("session-id-01", "Hi!", new WitContext());
        }

        private static WitContext Send(ConverseRequest request, ConverseResponse response)
        {
            Console.WriteLine(response.Msg);
            return request.Context;
        }

        private static WitContext SelectJoke(ConverseRequest request, ConverseResponse response)
        {
            var context = request.Context;

            var jokes = allJokes[context["cat"] || "default"];
            Shuffle(jokes);
            context["joke"] = jokes[0];
            return context;
        }

        private static WitContext Merge(ConverseRequest request, ConverseResponse response)
        {
            var context = request.Context;
            var entities = request.Entities;

            if (context.ContainsKey("joke"))
            {
                context.Remove("joke");
            }

            var category = FirstEntityValue(entities, "category");
            if (category != null)
            {
                context["cat"] = category;
            }

            var sentiment = FirstEntityValue(entities, "sentiment");
            if (sentiment != null)
            {
                context["ack"] = (sentiment.Equals("positive")) ? "Glad you liked it." : "Hmm.";
            } else if (context.ContainsKey("ack"))
            {
                context.Remove("ack");
            }
            return context;
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



        public static void Shuffle(List<string> list)
        {
            int n = list.Count;
            Random rnd = new Random();
            while (n > 1)
            {
                int k = (rnd.Next(0, n) % n);
                n--;
                string value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
