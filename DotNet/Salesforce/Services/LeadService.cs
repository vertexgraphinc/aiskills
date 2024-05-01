using Newtonsoft.Json;
using Salesforce.Contracts;
using Salesforce.DTOs;
using Salesforce.Helpers;
using Salesforce.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Collections;
using System.Net.NetworkInformation;

namespace Salesforce.Services
{
    public class LeadService : ILeadService
    {
        private readonly ApiHelper _apiHelper;

        public LeadService(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        private async Task<SalesforceLeads> QueryRawLeads(LeadQueryRequest request, string token)
        {
            string url = "v60.0/query?q=SELECT Id,+FirstName,+LastName,+Email,+Company,+Phone,+Status,+Industry,+LeadSource,+Rating,+Description+FROM+Lead" + Uri.EscapeDataString(BuildLeadQuery(request));
            SalesforceLeads result = await _apiHelper.Get<SalesforceLeads>(url, token);
            return result;
        }

        public async Task<List<LeadResponse>> QueryLeads(LeadQueryRequest request, string token)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][LeadService][QueryLeads]");

                SalesforceLeads Leads = await QueryRawLeads(request, token);
                if (Leads == null || Leads.Records == null)
                    return new List<LeadResponse>();

                List<LeadResponse> result = Leads.Records.Select(lead => new LeadResponse
                {
                    FirstName = lead.FirstName,
                    LastName = lead.LastName,
                    Phone = lead.Phone,
                    Email = lead.Email,
                    Description = lead.Description,
                    Company = lead.Company,
                    Status = lead.Status,
                    Industry = lead.Industry,
                    LeadSource = lead.LeadSource,
                    AnnualRevenue = lead.AnnualRevenue,
                    Rating = lead.Rating
                }).ToList();

                System.Diagnostics.Debug.WriteLine("[vertex][LeadService][QueryLeads]return:" + JsonConvert.SerializeObject(result));
                return result;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> CreateLead(LeadCreateRequest request, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(request.LastName))
                    throw new ArgumentException("Lead last name is not specified.");
                if (string.IsNullOrEmpty(request.Company))
                    throw new ArgumentException("Lead Company is not specified.");
                if (string.IsNullOrEmpty(request.Status))
                    throw new ArgumentException("Lead Status is not specified.");

                string url = "v60.0/sobjects/Lead";

                // Post the request to Salesforce API
                bool leadCreated = await _apiHelper.Post<bool>(url, request, token);

                // Check if the lead was successfully created
                if (!leadCreated)
                    throw new Exception("Failed to create lead.");

                return leadCreated;
            }
            catch (Exception ex)
            {
                // Log and rethrow the exception
                System.Diagnostics.Debug.WriteLine("[vertex][LeadService][CreateLead] Error: " + ex.Message);
                throw;
            }
        }

        

        public async Task<bool> UpdateLeads(LeadsUpdateRequest request, string token)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][LeadService][UpdateLeads]");

                if (request.GetType().GetProperties().All(property => property.GetValue(request) == null))
                    throw new Exception("Parameters are not specified.");

                SalesforceLeads Leads = await QueryRawLeads(new LeadQueryRequest
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Phone = request.Phone,
                    Email = request.Email,
                    Description = request.Description,
                    Company = request.Company,
                    Status = request.Status,
                    Industry = request.Industry,
                    LeadSource = request.LeadSource,
                    MinAnnualRevenue = request.MinAnnualRevenue,
                    MaxAnnualRevenue = request.MaxAnnualRevenue,
                    Rating = request.Rating
                }, token);
                if (Leads == null || Leads.Records == null)
                    throw new Exception("Leads not found.");

                var tasks = Leads.Records.Select(async Lead =>
                {
                    string urlQuery = $"v60.0/sobjects/Lead/{Lead.Id}";
                    object body = new
                    {
                        FirstName = string.IsNullOrEmpty(request.UpdatedFirstName) ? Lead.FirstName : request.UpdatedFirstName,
                        LastName = string.IsNullOrEmpty(request.UpdatedLastName) ? Lead.LastName : request.UpdatedLastName,
                        Email = string.IsNullOrEmpty(request.UpdatedEmail) ? Lead.Email : request.UpdatedEmail,
                        Company = string.IsNullOrEmpty(request.UpdatedCompany) ? Lead.Company : request.UpdatedCompany,
                        Phone = string.IsNullOrEmpty(request.UpdatedPhone) ? Lead.Phone : request.UpdatedPhone,
                        Status = string.IsNullOrEmpty(request.UpdatedStatus) ? Lead.Status : request.UpdatedStatus,
                        Industry = string.IsNullOrEmpty(request.UpdatedIndustry) ? Lead.Industry : request.UpdatedIndustry,
                        LeadSource = string.IsNullOrEmpty(request.UpdatedLeadSource) ? Lead.LeadSource : request.UpdatedLeadSource,
                        AnnualRevenue = string.IsNullOrEmpty(request.UpdatedAnnualRevenue) ? Lead.AnnualRevenue : request.UpdatedAnnualRevenue,
                        Rating = string.IsNullOrEmpty(request.Rating) ? Lead.Rating : request.Rating,
                        Description = string.IsNullOrEmpty(request.UpdatedDescription) ? Lead.Description : request.UpdatedDescription
                    };
                    return await _apiHelper.Patch(urlQuery, body, token);
                });
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any() || !results.All(result => result == true))
                    throw new Exception("One or more Leads failed to update.");

                System.Diagnostics.Debug.WriteLine("[vertex][LeadService][UpdateLeads]return:" + JsonConvert.SerializeObject(true));
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> RemoveLeads(LeadQueryRequest request, string token)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine("[vertex][LeadService][RemoveLeads]");

                SalesforceLeads Leads = await QueryRawLeads(request, token);
                if (Leads == null || Leads.Records == null)
                    throw new Exception("Leads not found.");

                var tasks = Leads.Records.Select(async Lead =>
                {
                    string urlQuery = $"v60.0/sobjects/Lead/{Lead.Id}";
                    return await _apiHelper.Delete(urlQuery, token);
                });
                var results = await Task.WhenAll(tasks);
                if (results == null || !results.Any() || !results.All(result => result == true))
                    throw new Exception("One or more Leads failed to remove.");

                System.Diagnostics.Debug.WriteLine("[vertex][LeadService][RemoveLeads]return:" + JsonConvert.SerializeObject(true));
                return true;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }


        private string BuildLeadQuery(LeadQueryRequest request)
        {
            var query = " WHERE ";

            if (!string.IsNullOrEmpty(request.FirstName))
            {
                query += $"FirstName = '{request.FirstName}' AND ";
            }

            if (!string.IsNullOrEmpty(request.LastName))
            {
                query += $"LastName = '{request.LastName}' AND ";
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                query += $"Email = '{request.Email}' AND ";
            }

            if (!string.IsNullOrEmpty(request.Company))
            {
                query += $"Company LIKE '%{request.Company}%' AND ";
            }

            if (!string.IsNullOrEmpty(request.Phone))
            {
                query += $"Phone = '{request.Phone}' AND ";
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                query += $"Status = '{request.Status}' AND ";
            }

            if (!string.IsNullOrEmpty(request.Industry))
            {
                query += $"Industry LIKE '%{request.Industry}%' AND ";
            }

            if (!string.IsNullOrEmpty(request.LeadSource))
            {
                query += $"LeadSource = '{request.LeadSource}' AND ";
            }

            if (!string.IsNullOrEmpty(request.Rating))
            {
                query += $"Rating = '{request.Rating}' AND ";
            }

            if (!string.IsNullOrEmpty(request.MinAnnualRevenue))
            {
                query += $"AnnualRevenue >= '{request.MinAnnualRevenue}' AND ";
            }

            if (!string.IsNullOrEmpty(request.MaxAnnualRevenue))
            {
                query += $"AnnualRevenue <= '{request.MaxAnnualRevenue}' AND ";
            }

            if (!string.IsNullOrEmpty(request.Description))
            {
                query += $"Description LIKE '%{request.Description}%' AND ";
            }

            if (query.EndsWith(" AND "))
            {
                query = query.Substring(0, query.Length - 5);
            }

            if (query.Equals(" WHERE ")) return "";

            return query;
        }
    }
}
