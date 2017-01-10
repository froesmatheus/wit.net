using MagisterBotApi.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MagisterBotApi
{
    public class Wit
    {
        private RestClient client;
        private Dictionary<int, int> Sessions;
        private Dictionary<dynamic, dynamic> Actions;

        public Wit(string accessToken)
        {
            client = new RestClient("https://api.wit.ai/");
            client.AddDefaultHeader("Authorization", string.Format("Bearer {0}", accessToken));
            client.AddDefaultHeader("Content-Type", "application/json");
            client.AddDefaultHeader("Accept", "application/json");
            client.AddDefaultParameter("v", "20170107", ParameterType.QueryString);
            client.AddDefaultParameter("session_id", "123abc", ParameterType.QueryString);

            Sessions = new Dictionary<int, int>();
            Actions = new Dictionary<dynamic, dynamic>();
        }

        public MessageResponse Message(MessageRequest message)
        {
            var request = new RestRequest("message", Method.GET);
            request.AddQueryParameter("q", message.Query);

            IRestResponse responseObject = client.Execute(request);
            MessageResponse response = JsonConvert.DeserializeObject<MessageResponse>(responseObject.Content);

            return response;
        }

        public ConverseResponse Converse(ConverseRequest converse)
        {
            if (converse.Context == null)
            {
                converse.Context = new Dictionary<string, dynamic>();
            }

            var request = new RestRequest("converse", Method.POST);
            request.AddQueryParameter("q", converse.Message);

            IRestResponse responseObject = client.Execute(request);
            ConverseResponse response = JsonConvert.DeserializeObject<ConverseResponse>(responseObject.Content);

            return response;
        }

        public void RunActions(ConverseRequest message)
        {
            if (message.Context == null)
            {
                message.Context = new Dictionary<string, dynamic>();
            }

            int currentRequest = 1;
            if (Sessions.ContainsKey(message.SessionId))
            {
                currentRequest = Sessions[message.SessionId] + 1;
            }
            Sessions[message.SessionId] = currentRequest;
        }


        private Dictionary<string, dynamic> RunActions(int sessionId, int currentRequest, string message, Dictionary<string, dynamic> context)
        {
            ConverseResponse response = Converse(new ConverseRequest(sessionId, message, context));

            if (currentRequest != Sessions[sessionId])
            {
                return context;
            }

            ConverseRequest request = new ConverseRequest();

            request.SessionId = sessionId;
            request.Context = context;
            request.Message = message;
            request.Entities = response.Entities;


            switch(response.Type)
            {

                case "msg":
                    this.Actions["send"] = response;
                    break;
                case "action":
                    string action = response.Action;
                    context = Actions[action];
                    break;
                case "stop":
                    return context;

            }


            return RunActions(sessionId, currentRequest, null, context);
        }
    }
}