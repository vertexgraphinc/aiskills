using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Zoho.Contracts;
using Zoho.DTOs;
using static Zoho.DTOs.ZohoDTOs;

namespace Zoho.Helpers
{
    public static class QueryHelper
    {
        public static int? ValidateReceivedInDays(int? receivedInDays)
        {
            if (!receivedInDays.HasValue || receivedInDays > 90)
            {
                return null;
            }
            else if (receivedInDays.Value < 16)
            {
                return 15;
            }
            else if (receivedInDays.Value > 15 && receivedInDays.Value <= 30)
            {
                return 30;
            }
            else
            {
                return 90;
            }
        }

        public static string ValidateSortBy(string sortBy)
        {
            sortBy = sortBy?.Trim();
            var validValues = new List<string> { "responseDueDate", "customerResponseTime", "createdTime" };
            return validValues.Contains(sortBy) ? sortBy : null;
        }

        public static string ValidateCommentSortBy(string sortBy)
        {
            sortBy = sortBy?.Trim();
            var validValues = new List<string> { "commentedTime", "-commentedTime" };
            return validValues.Contains(sortBy) ? sortBy : null;
        }

        public static string ValidateInclude(string include)
        {
            if (string.IsNullOrWhiteSpace(include))
            {
                return null;
            }

            var validValues = new List<string> { "contacts", "products", "departments", "team", "isRead", "assignee" };
            var includeItems = include.Split(',').Select(item => item.Trim()).ToList();

            foreach (var item in includeItems)
            {
                if (!validValues.Contains(item))
                {
                    return null;
                }
            }

            return string.Join(",", includeItems);
        }

        public static string ValidateCommentInclude(string include)
        {
            if (string.IsNullOrWhiteSpace(include))
            {
                return null;
            }

            var validValues = new List<string> { "mentions", "plainText" };
            var includeItems = include.Split(',').Select(item => item.Trim()).ToList();

            foreach (var item in includeItems)
            {
                if (!validValues.Contains(item))
                {
                    return null;
                }
            }

            return string.Join(",", includeItems);
        }

        public static async Task<List<string>> ValidateDepartmentIdsAsync(HttpClient client, string deptIds)
        {
            var validDeptIds = new List<string>();

            var response = await client.GetAsync("departments");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var departmentResponse = JsonConvert.DeserializeObject<DepartmentResponse>(content);
                var allDepts = departmentResponse.Data;

                if (allDepts.Count == 1)
                {
                    validDeptIds.Add(allDepts.First().Id.ToString());
                    return validDeptIds;
                }

                if (string.IsNullOrEmpty(deptIds))
                {
                    return validDeptIds;
                }

                var deptIdsArray = deptIds.Split(',');

                foreach (var deptIdOrName in deptIdsArray)
                {
                    var trimmedIdOrName = deptIdOrName.Trim();

                    var matchingDept = allDepts.FirstOrDefault(d => d.Id.ToString() == trimmedIdOrName || d.Name.Equals(trimmedIdOrName, StringComparison.OrdinalIgnoreCase));
                    if (matchingDept != null)
                    {
                        validDeptIds.Add(matchingDept.Id.ToString());
                    }
                }
            }

            return validDeptIds;
        }

        public static async Task<List<string>> ValidateTeamIdsAsync(HttpClient client, string teamIds)
        {
            var validTeamIds = new List<string>();
            var teamIdsArray = teamIds.Split(',');

            foreach (var teamIdOrName in teamIdsArray)
            {
                var trimmedIdOrName = teamIdOrName.Trim();

                var teamResponse = await client.GetAsync($"teams/{trimmedIdOrName}");
                if (teamResponse.IsSuccessStatusCode)
                {
                    validTeamIds.Add(trimmedIdOrName);
                }
                else
                {
                    var teamIdByName = await GetTeamIdByNameAsync(client, trimmedIdOrName);
                    if (teamIdByName != null && !string.IsNullOrEmpty(teamIdByName.ToString()))
                    {
                        validTeamIds.Add(teamIdByName.ToString());
                    }
                }
            }

            return validTeamIds;
        }

        public static async Task<List<string>> ValidateContactIdsAsync(HttpClient client, string contactIds)
        {
            var validContactIds = new List<string>();

            if (contactIds.Contains("\"data\":"))
            {
                try
                {
                    var contactInfo = JsonConvert.DeserializeObject<ContactDataResponse>(contactIds);
                    if (contactInfo?.Data != null && contactInfo.Data.Count > 0)
                    {
                        validContactIds.Add(contactInfo.Data.First().Id.ToString());
                        return validContactIds;
                    }
                }
                catch
                {
                    return validContactIds;
                }
            }

            var contactIdsArray = contactIds.Split(',');

            foreach (var contactIdOrName in contactIdsArray)
            {
                var trimmedIdOrName = contactIdOrName.Trim();

                var contactResponse = await client.GetAsync($"contacts/{trimmedIdOrName}");
                if (contactResponse.IsSuccessStatusCode)
                {
                    validContactIds.Add(trimmedIdOrName);
                }
                else
                {
                    var contactIdByName = await GetContactIdByNameAsync(client, trimmedIdOrName);
                    if (contactIdByName != null && !string.IsNullOrEmpty(contactIdByName.ToString()))
                    {
                        validContactIds.Add(contactIdByName.ToString());
                    }
                }
            }

            return validContactIds;
        }

        private static async Task<string> GetContactIdByNameAsync(HttpClient client, string searchStr)
        {
            var response = await client.GetAsync($"contacts/search?searchStr={searchStr}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var contacts = JsonConvert.DeserializeObject<List<ContactResponse>>(content);

                return contacts.FirstOrDefault()?.Id.ToString();
            }

            return null;
        }

        public static Contact ParseContact(string contactString)
        {
            var contact = new Contact();
            var parts = contactString.Split(' ');
            var emailPart = parts.FirstOrDefault(part => part.Contains("@"));

            if (emailPart != null)
            {
                contact.Email = emailPart.Trim();
                var nameParts = parts.Where(part => part != emailPart).ToArray();

                if (nameParts.Length == 1)
                {
                    contact.LastName = nameParts[0].Trim();
                }
                else if (nameParts.Length > 1)
                {
                    contact.FirstName = string.Join(" ", nameParts.Take(nameParts.Length - 1).Select(part => part.Trim()));
                    contact.LastName = nameParts.Last().Trim();
                }
            }
            else
            {
                if (parts.Length == 1)
                {
                    contact.LastName = parts[0].Trim();
                }
                else
                {
                    contact.FirstName = string.Join(" ", parts.Take(parts.Length - 1).Select(part => part.Trim()));
                    contact.LastName = parts.Last().Trim();
                }
            }

            return contact;
        }

        public static CreateTicket InitCreateTicket(CreateUpdateTicketRequest request)
        {
            var createTicket = new CreateTicket
            {
                Subject = request.Subject,
                DepartmentId = request.DepartmentId,
                ContactId = request.ContactId,
                ProductId = request.ProductId,
                Email = request.Email,
                Phone = request.Phone,
                Description = request.Description,
                Status = request.Status,
                AssigneeId = request.AssigneeId,
                Category = request.Category,
                Resolution = request.Resolution,
                DueDate = request.DueDate,
                Priority = request.Priority,
                Language = request.Language,
                ResponseDueDate = request.ResponseDueDate,
                Channel = request.Channel,
                Classification = request.Classification,
                TeamId = request.TeamId,
                Contact = new Contact()
            };

            return createTicket;
        }

        static async Task<Dictionary<long, string>> GetAllTeamsAsync(HttpClient client)
        {
            var response = await client.GetAsync("teams");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var teams = System.Text.Json.JsonSerializer.Deserialize<List<ZohoDTOs.Team>>(content);

            var teamDictionary = new Dictionary<long, string>();
            foreach (var team in teams)
            {
                teamDictionary[team.Id] = team.Name;
            }
            return teamDictionary;
        }

        static async Task<long?> GetTeamIdByNameAsync(HttpClient client, string teamName)
        {
            var teams = await GetAllTeamsAsync(client);
            foreach (var team in teams)
            {
                if (team.Value.Equals(teamName, StringComparison.OrdinalIgnoreCase))
                {
                    return team.Key;
                }
            }
            return null;
        }

        public static async Task<List<string>> ValidateAgentIdsAsync(HttpClient client, string agentIds)
        {
            var validAgentsIds = new List<string>();

            if (agentIds.Contains("\"data\":"))
            {
                try
                {
                    var contactInfo = JsonConvert.DeserializeObject<AgentDataResponse>(agentIds);
                    if (contactInfo?.Data != null && contactInfo.Data.Count > 0)
                    {
                        validAgentsIds.Add(contactInfo.Data.First().Id.ToString());
                        return validAgentsIds;
                    }
                }
                catch
                {
                    return validAgentsIds;
                }
            }

            var agentIdsArray = agentIds.Split(',');

            foreach (var agentIdOrName in agentIdsArray)
            {
                var trimmedIdOrName = agentIdOrName.Trim();

                var agentResponse = await client.GetAsync($"agents/{trimmedIdOrName}");
                if (agentResponse.IsSuccessStatusCode)
                {
                    validAgentsIds.Add(trimmedIdOrName);
                }
                else
                {
                    var agentIdByName = await SearchAgentId(client, trimmedIdOrName);
                    if (agentIdByName != null && !string.IsNullOrEmpty(agentIdByName.ToString()))
                    {
                        validAgentsIds.Add(agentIdByName.ToString());
                    }
                }
            }

            return validAgentsIds;
        }

        static async Task<long?> SearchAgentId(HttpClient client, string searchStr)
        {
            var response = await client.GetAsync($"agents?searchStr={searchStr}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var agents = JsonConvert.DeserializeObject<List<ZohoDTOs.Agent>>(content);

                return agents.FirstOrDefault()?.Id;
            }

            return null;
        }

        public static async Task<List<string>> ValidateChannelIdsAsync(HttpClient client, string channelIds)
        {
            var validChannelIds = new List<string>();
            var channelIdsArray = channelIds.Split(',');

            foreach (var channelIdOrName in channelIdsArray)
            {
                var trimmedIdOrName = channelIdOrName.Trim();
                var channels = await GetAllChannelsAsync(client);
                foreach (var channel in channels)
                {
                    if (channel.Value.Equals(trimmedIdOrName, StringComparison.OrdinalIgnoreCase) || channel.Key.ToString().Equals(trimmedIdOrName, StringComparison.OrdinalIgnoreCase))
                    {
                        validChannelIds.Add(channel.Key.ToString());
                    }
                }
            }

            return validChannelIds;
        }

        static async Task<Dictionary<string, string>> GetAllChannelsAsync(HttpClient client)
        {
            var response = await client.GetAsync("channels");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var channels = System.Text.Json.JsonSerializer.Deserialize<List<ZohoDTOs.Channel>>(content);

            var channelDictionary = new Dictionary<string, string>();
            foreach (var channel in channels)
            {
                channelDictionary[channel.Code] = channel.Name;
            }
            return channelDictionary;
        }

        public static async Task<string> ValidateTicketIdAsync(HttpClient client, string ticket_id_or_number)
        {
            var response = await client.GetAsync($"tickets/{ticket_id_or_number}");
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                response = await client.GetAsync($"tickets/search?ticketNumber={ticket_id_or_number}");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var ticketData = JsonDocument.Parse(content);
                    if (ticketData.RootElement.TryGetProperty("data", out var dataElement) && dataElement.ValueKind == JsonValueKind.Array)
                    {
                        var firstTicket = dataElement[0];
                        if (firstTicket.TryGetProperty("id", out var idElement))
                        {
                            return idElement.GetString();
                        }
                    }
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }
            else if (response.IsSuccessStatusCode)
            {
                return ticket_id_or_number;
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }

            throw new Exception("Ticket not found with the provided ID or number.");
        }

        public static bool? ParseBoolean(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            value = value.ToLower().Trim();

            if (value == "true" || value == "1")
            {
                return true;
            }
            else if (value == "false" || value == "0")
            {
                return false;
            }
            else
            {
                throw new ArgumentException("Invalid boolean value");
            }
        }

    }

}
