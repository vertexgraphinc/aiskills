using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Outlook.DTOs;
using Outlook.Contracts;
using Outlook.Interfaces;
using Outlook.Helpers;
using System.Linq;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;


namespace Outlook.Services
{
    public class MailService : IMailService
    {
        private readonly ApiHelper _apiHelper;

        public MailService(ApiHelper apiHelper)
        {
            _apiHelper = apiHelper;
        }

        #region Message
        public async Task<List<EmailResponse>> ListMessage(QueryEmailsRequest request, string token)
        {
            string query = "filter=";

            string beginDate = UtilityHelper.FormatDateTimeUtc(DateTime.Now.AddDays(-1));
            string endDate = UtilityHelper.FormatDateTimeUtc(DateTime.Now);
            if (!string.IsNullOrEmpty(request.BeginTime))
            {
                DateTime BeginDT = DateTime.Parse(request.BeginTime);
                beginDate = UtilityHelper.FormatDateTimeUtc(BeginDT);

                query += $"receivedDateTime ge {beginDate}";
            }
            if (!string.IsNullOrEmpty(request.EndTime))
            {
                DateTime EndDT = DateTime.Parse(request.EndTime);
                endDate = UtilityHelper.FormatDateTimeUtc(EndDT);

                if (query != "filter=")
                {
                    query += " and ";
                }
                query += $"receivedDateTime le {endDate}";
            }

            if (!string.IsNullOrEmpty(request.From))
            {
                if (query != "filter=")
                {
                    query += " and ";
                }
                query += $"(contains(from/emailAddress/address, \'{request.From}\') or contains(from/emailAddress/name, '{request.From}'))";
            }

            if (!string.IsNullOrEmpty(request.Subject))
            {
                if (query != "filter=")
                {
                    query += " and ";
                }
                query += $"contains(subject, \'{request.Subject}\')";
            }

            if (!string.IsNullOrEmpty(request.Importance))
            {
                if (query != "filter=")
                {
                    query += " and ";
                }
                query += $"importance eq \'{request.Importance}\'";
            }

            if (request.HasAttachments != null)
            {
                if (query != "filter=")
                {
                    query += " and ";
                }

                if ((bool)request.HasAttachments)
                {
                    query += "hasAttachments eq true";
                }
                else
                {
                    query += "hasAttachments eq false";
                }
            }

            if (!string.IsNullOrEmpty(request.Body))
            {
                // cannot filter by body content, can search though, but what else can we send in the search query?
                query = $"$search={request.Body}";
            }

            if (!string.IsNullOrEmpty(request.To))
            {
                // cannot filter by sender, can search though, but what else can we send in the search query?
                query = $"$search=\"recipients:{request.To}\"";
            }

            try
            {
                MSGraphMessages msgs = await _apiHelper.Get<MSGraphMessages>($"messages?{query}", token);
                if (msgs == null || msgs.Values == null)
                    return new List<EmailResponse>();

                return msgs.Values.Select(msg => new EmailResponse
                {
                    Id = msg.Id,
                    From = msg.From.EmailAddress.Address,
                    Subject = msg.Subject,
                    Body = msg.BodyPreview, //Utils.CleanHtml(msg.BodyPreview);
                    Received = msg.ReceivedDateTime.ToString()
                }).ToList();
            }
            catch (HttpRequestException e)
            {
                // Handle exception (e.g., log it)
                return new List<EmailResponse>();
            }
        }

        public async Task<EmailResponse> GetMessage(EmailIdRequest request, string token)
        {
            string query = $"messages/{request.Id}";

            try
            {
                MSGraphMessage msg = await _apiHelper.Get<MSGraphMessage>(query, token);
                if (msg == null)
                    return new EmailResponse();

                return new EmailResponse
                {
                    Id = msg.Id,
                    From = msg.From.EmailAddress.Address,
                    Subject = msg.Subject,
                    Body = msg.BodyPreview, //Utils.CleanHtml(msg.BodyPreview);
                    Received = msg.ReceivedDateTime.ToString()
                };
            }
            catch (HttpRequestException e)
            {
                // Handle exception (e.g., log it)
                return new EmailResponse();
            }
        }

        public async Task<bool> DeleteMessage(EmailIdRequest request, string token)
        {
            string query = $"messages/{request.Id}";

            try
            {
                return await _apiHelper.Delete(query, token);
            }
            catch (HttpRequestException e)
            {
                // Handle exception (e.g., log it)
                return false;
            }
        }

        public async Task<bool> SendDraftMessage(EmailIdRequest request, string token)
        {
            string query = $"messages/{request.Id}/send";

            try
            {
                bool success = await _apiHelper.Post<bool>(query, null, token);
                return success;
            }
            catch (HttpRequestException e)
            {
                // Handle exception (e.g., log it)
                return false;
            }
        }


        public async Task<bool> ReplyMessage(EmailReplyRequest request, string token)
        {
            string query = $"messages/{request.Id}/reply";

            var toRL = UtilityHelper.GetEmailListFromString(request.ToRecipients);
            var toR = toRL.Select(email => new
            {
                emailAddress = new { address = email }
            }).ToArray();

            var requestBody = new
            {
                message = new
                {
                    toRecipients = toR
                },
                comment = request.Comment ?? ""
            };

            try
            {
                bool success = await _apiHelper.Post<bool>(query, requestBody, token);
                return success;
            }
            catch (HttpRequestException e)
            {
                // Handle exception (e.g., log it)
                return false;
            }
        }

        public async Task<bool> ReplyAllMessage(EmailReplyAllRequest request, string token)
        {
            string query = $"messages/{request.Id}/replyAll";

            var requestBody = new
            {
                comment = request.Comment ?? ""
            };

            try
            {
                bool success = await _apiHelper.Post<bool>(query, requestBody, token);
                return success;
            }
            catch (HttpRequestException e)
            {
                // Handle exception (e.g., log it)
                return false;
            }
        }

        public async Task<bool> ForwardMessage(EmailForwardRequest request, string token)
        {
            string query = $"messages/{request.Id}/forward";

            var toRL = UtilityHelper.GetEmailListFromString(request.ToRecipients);
            var toR = toRL.Select(email => new
            {
                emailAddress = new { address = email }
            }).ToArray();

            var requestBody = new
            {
                toRecipients = toR,
                comment = request.Comment ?? ""
            };

            try
            {
                bool success = await _apiHelper.Post<bool>(query, requestBody, token);
                return success;
            }
            catch (HttpRequestException e)
            {
                // Handle exception (e.g., log it)
                return false;
            }
        }

        public async Task<bool> SendMail(EmailRequest request, string Token)
        {
            var toRL = UtilityHelper.GetEmailListFromString(request.ToRecipients);
            var toR = toRL.Select(email => new
            {
                emailAddress = new { address = email }
            }).ToArray();

            var ccRL = UtilityHelper.GetEmailListFromString(request.CCRecipients);
            var ccR = ccRL.Select(email => new
            {
                emailAddress = new { address = email }
            }).ToArray();

            var bccRL = UtilityHelper.GetEmailListFromString(request.BCCRecipients);
            var bccR = bccRL.Select(email => new
            {
                emailAddress = new { address = email }
            }).ToArray();

            var requestBody = new
            {
                message = new
                {
                    subject = request.Subject ?? "",
                    body = new
                    {
                        contentType = "text",
                        content = request.Body ?? ""
                    },
                    toRecipients = toR,
                    ccRecipients = ccR,
                    bccRecipients = bccR
                }
            };

            try
            {
                bool success = await _apiHelper.Post<bool>("sendMail", requestBody, Token);
                return success;
            }
            catch (HttpRequestException e)
            {
                // Handle exception (e.g., log it)
                return false;
            }
        }

        #endregion


    }
}
