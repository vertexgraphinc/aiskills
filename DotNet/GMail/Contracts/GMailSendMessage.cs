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
    public class GMailSendMessage
    {
        string _to = "";
        string _subject = "";
        string _body = "";

        [JsonProperty("to")]
        [JsonPropertyName("to")]
        public string To
        {
            get { return _to; }
            set { _to = value; }
        }

        [JsonProperty("subject")]
        [JsonPropertyName("subject")]
        public string Subject
        {
            get { return _subject; }
            set { _subject = value; }
        }

        [JsonProperty("body")]
        [JsonPropertyName("body")]
        public string Body
        {
            get { return _body; }
            set { _body = value; }
        }
    }
}
