using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace GMail.GMailClient
{
    public class GMailClientLabel
    {
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("messageListVisibility", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("messageListVisibility")]
        public string MessageListVisibility { get; set; } //show, hide
        
        [JsonProperty("labelListVisibility", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("labelListVisibility")]
        public string LabelListVisibility { get; set; } //labelShow, labelShowIfUnread, labelHide

        //label create properties----------------------------------------------

        [JsonProperty("messagesTotal", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("messagesTotal")]
        public int MessagesTotal { get; set; }

        [JsonProperty("messagesUnread", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("messagesUnread")]
        public int MessagesUnread { get; set; }

        [JsonProperty("threadsTotal", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("threadsTotal")]
        public int ThreadsTotal { get; set; }

        [JsonProperty("threadsUnread", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("threadsUnread")]
        public int ThreadsUnread { get; set; }

        [JsonProperty("color", DefaultValueHandling = DefaultValueHandling.Ignore)]
        [JsonPropertyName("color")]
        public GMailClientColor Color { get; set; }

    }
}
