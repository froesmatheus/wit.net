using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WitAi.Models;

namespace WitAi
{
    public class Wit
    {
        public string WIT_API_HOST = "https://api.wit.ai";
        public string WIT_API_VERSION = "20170107";
        public string LEARN_MORE = "Learn more at https://wit.ai/docs/quickstart";


        private RestClient client;
        private Dictionary<string, int> sessions;
        private WitAction actions = new WitAction();

        /// <summary>
        /// Initializes new instance of Wit class
        /// </summary>
        /// <param name="accessToken">Client access token. You can grab it from your Wit console, under Settings\Access Token.</param>
        public Wit(string accessToken, WitAction actions = null)
        {
            client = PrepareRestClient(accessToken);

            sessions = new Dictionary<string, int>();

            if (actions != null)
            {
                this.actions = ValidateActions(actions);
            }
        }

        private RestClient PrepareRestClient(string accessToken)
        {
            RestClient restClient = new RestClient(WIT_API_HOST);
            restClient.AddDefaultHeader("Authorization", $"Bearer {accessToken}");
            restClient.AddDefaultHeader("Content-Type", "application/json");
            restClient.AddDefaultHeader("Accept", "application/json");
            restClient.AddDefaultParameter("v", WIT_API_VERSION, ParameterType.QueryString);

            return restClient;
        }

        private WitAction ValidateActions(WitAction actions)
        {
            if (!actions.ContainsKey("send"))
            {
                Console.WriteLine("The send action is missing.");
            }

            return actions;
        }

        /// <summary>
        /// Capture intent and entities from a text string
        /// </summary>
        /// <param name="msg">Text string to capture from</param>
        /// <param name="verbose">Calls the API in Verbose Mode</param>
        /// <returns>Message response</returns>
        public MessageResponse Message(string msg, bool verbose = true)
        {
            var request = new RestRequest("message", Method.GET);
            request.AddQueryParameter("q", msg);

            IRestResponse responseObject = client.Execute(request);
            MessageResponse response = JsonConvert.DeserializeObject<MessageResponse>(responseObject.Content);

            return response;
        }

        public ConverseResponse Converse(string sessionId, string message, WitContext context, bool verbose = true)
        {
            if (context == null)
            {
                context = new WitContext();
            }

            var request = new RestRequest("converse", Method.POST);
            request.AddQueryParameter("q", message);
            request.AddQueryParameter("session_id", sessionId);

            IRestResponse responseObject = client.Execute(request);
            ConverseResponse response = JsonConvert.DeserializeObject<ConverseResponse>(responseObject.Content);

            return response;
        }

        public WitContext RunActions(string sessionId, string message, WitContext context,
                                                      int maxSteps = 5, bool verbose = true)
        {
            if (this.actions == null)
            {
                ThrowMustHaveActions();
            }


            if (context == null)
            {
                context = new WitContext();
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


        private WitContext _RunActions(string sessionId, int currentRequest,
                                                    string message, WitContext context, int maxSteps = 5, bool verbose = true)
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
                    ConverseResponse response = new ConverseResponse();
                    response.Msg = json.Msg;
                    response.QuickReplies = json.QuickReplies;
                    actions["send"](request, response);
                    break;
                case "action":
                    string action = json.Action;
                    context = this.actions[action](request, null);

                    if (context == null)
                    {
                        Console.WriteLine("missing context - did you forget to return it?");
                        context = new WitContext();
                    }
                    break;
                default:
                    throw new WitException($"unknown type:  {json.Type}");
            }

            if (currentRequest != sessions[sessionId])
            {
                return context;
            }

            return _RunActions(sessionId, currentRequest, null, context, maxSteps - 1, verbose);
        }

        private void ThrowIfActionMissing(string actionName)
        {
            if (!this.actions.ContainsKey("actionName"))
            {
                throw new WitException($"unknown action {actionName}");
            }
        }

        private void ThrowMustHaveActions()
        {
            throw new WitException($"You must provide the 'actions' parameter to be able to use runActions. ${LEARN_MORE}");
        }
    }
}