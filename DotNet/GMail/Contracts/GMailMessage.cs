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
    public class GMailMessage
    {
        string _id = "";
        string _threadId = "";
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
        [JsonProperty("thread_id")]
        [JsonPropertyName("thread_id")]
        public string ThreadId
        {
            get { return _threadId; }
            set { _threadId = value; }
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
            set {
                try
                {
                    DateTime dt = DateTime.Parse(value);
                    _received = dt.ToString("yyyy/MM/dd hh:mm:ss tt");
                }
                catch(Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("[vertex][GMailMessage][Received]:set:ex:" + ex.Message);
                    System.Diagnostics.Debug.WriteLine("[vertex][GMailMessage][Received]:value:" + value);
                    _received = value;
                }
            }
        }
    }
}
