using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace GMail.Contracts
{
    public class ServerResponse
    {
        string _messageFromServer = "";

        [JsonProperty("messageFromServer")]
        [JsonPropertyName("messageFromServer")]
        public string MessageFromServer
        {
            get { return _messageFromServer; }
            set { _messageFromServer = value; }
        }
    }
}
