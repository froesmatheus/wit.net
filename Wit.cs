using MagisterBotApi.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wit.NET;

namespace MagisterBotApi
{
    public class Wit
    {
        private string WIT_API_HOST = "https://api.wit.ai";
        private string WIT_API_VERSION = "20170107";
        private int DEFAULT_MAX_STEPS = 5;
        private string LEARN_MORE = "Learn more at https://wit.ai/docs/quickstart";


        private RestClient client;
        private Dictionary<int, int> sessions;
        private Dictionary<string, dynamic> actions;

        public Wit(string accessToken, Dictionary<string, dynamic> actions = null)
        {
            client = PrepareRestClient(accessToken);

            sessions = new Dictionary<int, int>();
            
            if (actions != null)
            {
                this.actions = ValidateActions(actions);
            }

            this.actions = new Dictionary<string, dynamic>();
        }

        private RestClient PrepareRestClient(string accessToken)
        {
            RestClient restClient = new RestClient(WIT_API_HOST);
            restClient.AddDefaultHeader("Authorization", string.Format("Bearer {0}", accessToken));
            restClient.AddDefaultHeader("Content-Type", "application/json");
            restClient.AddDefaultHeader("Accept", "application/json");
            restClient.AddDefaultParameter("v", WIT_API_VERSION, ParameterType.QueryString);

            return restClient;
        }

        private Dictionary<string, dynamic> ValidateActions(Dictionary<string, dynamic> actions)
        {
            if (!actions.ContainsKey("send"))
            {
                Console.WriteLine("The send action is missing.");
            }

            return actions;
        }

        public MessageResponse Message(string msg, bool verbose = true)
        {
            var request = new RestRequest("message", Method.GET);
            request.AddQueryParameter("q", msg);

            IRestResponse responseObject = client.Execute(request);
            MessageResponse response = JsonConvert.DeserializeObject<MessageResponse>(responseObject.Content);

            return response;
        }

        public ConverseResponse Converse(int sessionId, string message, Dictionary<string, dynamic> context, bool verbose = true)
        {
            if (context == null)
            {
                context = new Dictionary<string, dynamic>();
            }

            var request = new RestRequest("converse", Method.POST);
            request.AddQueryParameter("q", message);

            IRestResponse responseObject = client.Execute(request);
            ConverseResponse response = JsonConvert.DeserializeObject<ConverseResponse>(responseObject.Content);

            return response;
        }

        public Dictionary<string, dynamic> RunActions(int sessionId, string message, Dictionary<string, dynamic> context,
                                                      int maxSteps = 5, bool verbose = true)
        {
            if (context == null)
            {
                context = new Dictionary<string, dynamic>();
            }

            /** Figuring out whether we need to reset the last turn.
                Each new call increments an index for the session.
                We only care about the last call to run_actions.
                All the previous ones are discarded (preemptive exit).*/
            int currentRequest = 1;
            if (sessions.ContainsKey(sessionId))
            {
                currentRequest = sessions[sessionId] + 1;
            }
            sessions[sessionId] = currentRequest;

            context = _RunActions(sessionId, currentRequest, message, context, maxSteps, verbose);

            // Cleaning up once the last call to run_actions finishes.
            if (currentRequest == sessions[sessionId])
            {
                sessions.Remove(sessionId);
            }

            return context;
        }


        private Dictionary<string, dynamic> _RunActions(int sessionId, int currentRequest,
                                                    string message, Dictionary<string, dynamic> context, int maxSteps = 5, bool verbose = true)
        {

            if (maxSteps <= 0)
            {
                throw new WitException("Max steps reached, stopping.");
            }
            ConverseResponse json = Converse(sessionId, message, context);
            if (json.Type == null)
            {
                throw new WitException("Couldn\'t find type in Wit response");
            }

            if (currentRequest != sessions[sessionId])
            {
                return context;
            }



            // backwards-compatibility with API version 20160516
            if (json.Type == "merge")
            {
                json.Type = "action";
                json.Action = "merge";
            }

            if (json.Type == "error")
            {
                throw new Exception("Oops, I don\'t know what to do.");
            }

            if (json.Type == "stop")
            {
                return context;
            }


            ConverseRequest request = new ConverseRequest();

            request.SessionId = sessionId;
            request.Context = context;
            request.Message = message;
            request.Entities = json.Entities;


            switch (json.Type)
            {

                case "msg":
                    Dictionary<string, dynamic> response = new Dictionary<string, dynamic>();
                    response["text"] = json.Msg;
                    response["quickreplies"] = json.QuickReplies;
                    break;
                case "action":
                    string action = response.Action;
                    context = actions[action];
                    break;
                default:
                    throw new WitException($"unknown type:  {json.Type}");
            }

            if (currentRequest != sessions[sessionId])
            {
                return context;
            }

            return _RunActions(sessionId, currentRequest, null, context, maxSteps, verbose);
        }
    }
}