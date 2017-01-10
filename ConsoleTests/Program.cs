using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wit.Models;

namespace ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            var actions = new WitAction();
            actions["send"] = send;
            actions["getEndereco"] = getEndereco;

            Wit.Wit wit = new Wit.Wit("OI7YL4ZS5I7LGOKM5MKOVBRV6EALUYWK", actions);

            string sessionId = "user-5";
            WitContext context = new WitContext();

            WitContext context1 = wit.RunActions(sessionId, "Olá", context);
            WitContext context2 = wit.RunActions(sessionId, "Gostaria de consultar meu CEP", context1);
            WitContext context3 = wit.RunActions(sessionId, "Meu CEP é 49025390", context2);

            Console.ReadKey();
        }

        private static WitContext getEndereco(ConverseRequest request, ConverseResponse response)
        {
            request.Context.Remove("endereco");
            request.Context.Add("endereco", "Rua A");
            return request.Context;
        }

        private static WitContext send(ConverseRequest request, ConverseResponse response)
        {
            Console.WriteLine("Sending to user..." + response.Msg);
            return request.Context;
        }
    }
}
