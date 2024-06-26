﻿using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Jira.Contracts
{
    public class ServerResponse
    {
        string _message = "";

        [JsonProperty("message"),JsonPropertyName("message")]
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }
}
