using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Zoho.Contracts;

namespace Zoho.DTOs
{
    public class ZohoDTOs
    {
        public class Contact
        {
            [JsonProperty("firstName"), JsonPropertyName("firstName")]
            public string FirstName { get; set; }

            [JsonProperty("lastName"), JsonPropertyName("lastName")]
            public string LastName { get; set; }

            [JsonProperty("email"), JsonPropertyName("email")]
            public string Email { get; set; }

            [JsonProperty("phone"), JsonPropertyName("phone")]
            public string Phone { get; set; }
        }

        public class ContactResponse : Contact
        {
            [JsonProperty("id"), JsonPropertyName("id")]
            public long Id { get; set; }
        }

        public class DepartmentResponse
        {
            [JsonProperty("data")]
            public List<Department> Data { get; set; }
        }

        public class Department
        {
            [JsonProperty("id"), JsonPropertyName("id")]
            public long Id { get; set; }

            [JsonProperty("name"), JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonProperty("description"), JsonPropertyName("description")]
            public string Description { get; set; }
        }

        public class Team
        {
            [JsonProperty("id"), JsonPropertyName("id")]
            public long Id { get; set; }

            [JsonProperty("name"), JsonPropertyName("name")]
            public string Name { get; set; }

            [JsonProperty("description"), JsonPropertyName("description")]
            public string Description { get; set; }
        }

        public class Agent
        {
            [JsonProperty("id"), JsonPropertyName("id")]
            public long Id { get; set; }

            [JsonProperty("firstName"), JsonPropertyName("firstName")]
            public string FirstName { get; set; }

            [JsonProperty("lastName"), JsonPropertyName("lastName")]
            public string LastName { get; set; }

            [JsonProperty("emailId"), JsonPropertyName("emailId")]
            public string Email { get; set; }

            [JsonProperty("aboutInfo"), JsonPropertyName("aboutInfo")]
            public string AboutInfo { get; set; }
        }

        public class Channel
        {
            [JsonProperty("code"), JsonPropertyName("code")]
            public string Code { get; set; }

            [JsonProperty("name"), JsonPropertyName("name")]
            public string Name { get; set; }
        }

        public class CreateTicket : CreateUpdateTicketRequest
        {
            [JsonProperty("contact"), JsonPropertyName("contact")]
            public Contact Contact { get; set; }
        }

        public class ContactDataResponse
        {
            [JsonProperty("data")]
            public List<ContactResponse> Data { get; set; }
        }

        public class AgentDataResponse
        {
            [JsonProperty("data")]
            public List<Agent> Data { get; set; }
        }
    }

}
