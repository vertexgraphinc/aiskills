using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GMail.Contracts
{
    public class GMailMessage: ValidationHelpers
    {
        string _id = "";
        string _to = "";
        string _from = "";
        string _subject = "";
        string _snippet = "";
        string _body = "";
        string _received = "";

        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [JsonProperty("to")]
        [JsonPropertyName("to")]
        public string To
        {
            get { return _to; }
            set { _to = value; }
        }

        [JsonProperty("from")]
        [JsonPropertyName("from")]
        public string From
        {
            get { return _from; }
            set { _from = value; }
        }

        [JsonProperty("subject")]
        [JsonPropertyName("subject")]
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        [JsonProperty("snippet")]
        [JsonPropertyName("snippet")]
        public string Snippet
        {
            get { return _snippet; }
            set { _snippet = value; }
        }

        [JsonProperty("body")]
        [JsonPropertyName("body")]
        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }

        [JsonProperty("received")]
        [JsonPropertyName("received")]
        public string Received
        {
            get { return _received; }
            set { _received = value; }
        }
    }
}
