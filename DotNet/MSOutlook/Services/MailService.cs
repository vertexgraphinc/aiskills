using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MSOutlook.DTOs;
using MSOutlook.Contracts;
using MSOutlook.Interfaces;
using MSOutlook.Helpers;
using System.Linq;
using System.Net.Http;
using System.Runtime.ConstrainedExecution;


namespace MSOutlook.Services
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
            string query = "$filter=";

            string beginDate = UtilityHelper.FormatDateTimeUtc(DateTime.Now.AddDays(-1));
            string endDate = UtilityHelper.FormatDateTimeUtc(DateTime.Now);
            if (!string.IsNullOrEmpty(request.BeginTime))
            {
                DateTime BeginDT = DateTime.Parse(request.BeginTime);
                beginDate = UtilityHelper.FormatDateTimeUtc(BeginDT);

                query += $"receivedDateTime ge {beginDate} and ";
            }
            if (!string.IsNullOrEmpty(request.EndTime))
            {
                DateTime EndDT = DateTime.Parse(request.EndTime);
                endDate = UtilityHelper.FormatDateTimeUtc(EndDT);

                query += $"receivedDateTime le {endDate} and ";
            }

            if (!string.IsNullOrEmpty(request.From))
            {
                query += $"(contains(from/emailAddress/address, \'{request.From}\') or contains(from/emailAddress/name, '{request.From}')) and ";
            }

            if (!string.IsNullOrEmpty(request.Subject))
            {
                query += $"contains(subject, \'{request.Subject}\') and ";
            }

            if (!string.IsNullOrEmpty(request.Importance))
            {
                query += $"importance eq \'{request.Importance}\' and ";
            }

            if (request.HasAttachments != null)
            {

                if ("true".Equals(request.HasAttachments, StringComparison.CurrentCultureIgnoreCase) || "1".Equals(request.HasAttachments))
                {
                    query += "hasAttachments eq true and ";
                }
                else
                {
                    query += "hasAttachments eq false and ";
                }
            }

            if (!string.IsNullOrEmpty(request.To))
            {
                query += $"contains(recipients,\'{request.To}\') and ";
            }

            if (query.EndsWith(" and "))
            {
                query = query.Substring(0, query.Length - 5); // Removing the last " and "
            }

            try
            {
                MSGraphMessages msgs = await _apiHelper.Get<MSGraphMessages>($"messages?{query}", token);
                if (msgs == null || msgs.Values == null)
                    return new List<EmailResponse>();

                var msgList = msgs.Values;
                if (!string.IsNullOrEmpty(request.Body))
                {
                    msgList = msgList.Where(msg => msg.BodyPreview.Contains(request.Body)).ToList();
                }
                

                return msgList.Select(msg => new EmailResponse
                {
                    Id = msg.Id,
                    From = (msg.From != null && msg.From.EmailAddress != null ? msg.From.EmailAddress.Address : null),
                    Subject = msg.Subject,
                    Body = UtilityHelper.Sanitize(UtilityHelper.StripHtmlTags(msg.Body.Content)),
                    Received = msg.ReceivedDateTime.ToString()
                }).ToList();
            }
            catch (HttpRequestException e)
            {
                // Handle exception (e.g., log it)
                return new List<EmailResponse>();
            }
        }

        public async Task<bool> DeleteMessage(string msdId, string token)
        {
            string query = $"messages/{msdId}";

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

        public async Task<bool> ReplyMessage(string msgId, string recipients, string comment, string token)
        {
            string query = $"messages/{msgId}/reply";

            var toRL = UtilityHelper.GetEmailListFromString(recipients);
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
                comment = comment ?? ""
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

        public async Task<bool> ReplyMultipleMessages(EmailReplyRequest request, string msgId, string token)
        {
            bool allSuccess = true;
            var msgs = JsonHelper.GetEmailsResponseJsonObject(msgId);
            foreach (var msg in msgs)
            {
                if (!await ReplyMessage(msg.Id, msg.From, request.Comment, token))
                {
                    allSuccess = false;
                }
            }

            return allSuccess;
        }


        public async Task<bool> ReplyAllMessage(string msgId, string comment, string token)
        {
            string query = $"messages/{msgId}/replyAll";

            var requestBody = new
            {
                comment = comment ?? ""
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

        public async Task<bool> ReplyAllMultipleMessages(EmailReplyAllRequest request, string msgId, string token)
        {
            bool allSuccess = true;
            var msgs = JsonHelper.GetEmailsResponseJsonObject(msgId);
            foreach (var msg in msgs)
            {
                if (!await ReplyAllMessage(msg.Id, request.Comment, token))
                {
                    allSuccess = false;
                }
            }

            return allSuccess;
        }

        public async Task<bool> ForwardMessage(string msgId, string recipients, string comment, string token)
        {
            string query = $"messages/{msgId}/forward";

            var toRL = UtilityHelper.GetEmailListFromString(recipients);
            var toR = toRL.Select(email => new
            {
                emailAddress = new { address = email }
            }).ToArray();

            var requestBody = new
            {
                toRecipients = toR,
                comment = comment ?? ""
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

        public async Task<bool> ForwardMultipleMessages(EmailForwardRequest request, string msgId, string token)
        {
            bool allSuccess = true;
            var msgs = JsonHelper.GetEmailsResponseJsonObject(msgId);
            foreach (var msg in msgs)
            {
                if (!await ForwardMessage(msg.Id, request.ToRecipients, request.Comment, token))
                {
                    allSuccess = false;
                }
            }

            return allSuccess;
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
