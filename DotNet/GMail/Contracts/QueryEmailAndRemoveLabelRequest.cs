using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace GMail.Contracts
{
    public class QueryEmailAndRemoveLabelRequest : SearchFilters
    {
        [JsonProperty("remove_label"),JsonPropertyName("remove_label")]
        public string RemoveLabel { get; set; }

        public SearchFilters GetSearchFilters()
        {
            var sf = new SearchFilters();
            sf.From = this.From;
            sf.To = this.To;
            sf.Subject = this.Subject;
            sf.Body = this.Body;
            sf.Status = this.Status;
            sf.Label = this.Label;
            sf.BeginTime = this.BeginTime;
            sf.EndTime = this.EndTime;
            sf.Status = this.Status;
            return sf;
        }
    }
}
