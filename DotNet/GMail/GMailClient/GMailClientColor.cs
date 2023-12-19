using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using JsonIgnoreAttribute = System.Text.Json.Serialization.JsonIgnoreAttribute;

namespace GMail.GMailClient
{
    public class GMailClientColor
    {
        //https://developers.google.com/gmail/api/reference/rest/v1/users.labels#Color
        [JsonPropertyName("textColor")]
        [JsonProperty("textColor")]
        public string TextColor { get; set; }

        [JsonPropertyName("backgroundColor")]
        [JsonProperty("backgroundColor")]
        public string BackgroundColor { get; set; }

    }
}
