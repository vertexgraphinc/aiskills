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
        string _message = "";

        [JsonProperty("message")]
        [JsonPropertyName("message")]
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
    }
}
