using Microsoft.AspNetCore.Mvc;
using MSOutlook.Contracts;
using MSOutlook.Helpers;
using MSOutlook.Interfaces;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace MSOutlook.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMailService _mailService;

        public MessagesController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost("query")]
        public async Task<QueryEmailsResponse> ListMessages(QueryEmailsRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ListMessages]");
            QueryEmailsResponse resp = new QueryEmailsResponse
            {
                EmailMessages = null
            };

            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            resp.EmailMessages = await _mailService.ListMessage(request, token);
            resp.EmailMessages = resp.EmailMessages.Select(message =>
            {
                message.Id = null;
                return message;
            }).ToList();

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ListMessages] Response:" + JsonConvert.SerializeObject(resp));
            return resp;
        }

        [HttpPost("get")]
        public async Task<EmailResponse> GetMessage(EmailGetRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][GetMessage]");
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            var findEmailRequest = new QueryEmailsRequest
            {
                From = request.From,
                To = request.To,
                Subject = request.Subject,
                Body = request.Body
            };

            var emailList = await _mailService.ListMessage(findEmailRequest, token);

            if (emailList.Count == 0 || emailList.Count > 1)
            {
                Response.StatusCode = 500;
                return null;
            }

            var email = emailList[0];

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][GetMessage] Response:" + JsonConvert.SerializeObject(email));
            return email;
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteMessage(EmailDeleteRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][DeleteMessage]");
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            var findEmailRequest = new QueryEmailsRequest
            {
                From = request.From,
                To = request.To,
                Subject = request.Subject,
                Body = request.Body
            };

            var emailList = await _mailService.ListMessage(findEmailRequest, token);

            if (emailList.Count == 0 || emailList.Count > 1)
            {
                return StatusCode(500, "Email not found, give more specific information");
            }

            var id = emailList[0].Id;

            bool isDeleted = await _mailService.DeleteMessage(id, token);

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][DeleteMessage] Response:" + isDeleted);

            if (isDeleted)
            {
                return Ok("Message deleted successfully.");
            }
            else
            {
                return BadRequest("Failed to delete message.");
            }
        }

        [HttpPost("reply")]
        public async Task<IActionResult> ReplyMessage(EmailReplyRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ReplyMessage]");
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isSent;

            var findEmailRequest = new QueryEmailsRequest
            {
                From = request.From,
                To = request.To,
                Subject = request.Subject,
                Body = request.Body
            };

            var emailList = await _mailService.ListMessage(findEmailRequest, token);

            if (emailList.Count == 0 || emailList.Count > 1)
            {
                return StatusCode(500, "Email not found, give more specific information");
            }

            var id = emailList[0].Id;

            if (JsonHelper.IsJsonObject(id))
            {
                isSent = await _mailService.ReplyMultipleMessages(request, id, token);
            } else
            {
                isSent = await _mailService.ReplyMessage(id, request.ToRecipients, request.Comment, token);
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ReplyMessage] Response:" + isSent);

            if (isSent)
            {
                return Ok("Message sent successfully.");
            }
            else
            {
                return BadRequest("Failed to send message.");
            }
        }

        [HttpPost("replyAll")]
        public async Task<IActionResult> ReplyAllMessage(EmailReplyAllRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ReplyAllMessage]");
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isSent;

            var findEmailRequest = new QueryEmailsRequest
            {
                From = request.From,
                To = request.To,
                Subject = request.Subject,
                Body = request.Body
            };

            var emailList = await _mailService.ListMessage(findEmailRequest, token);

            if (emailList.Count == 0 || emailList.Count > 1)
            {
                return StatusCode(500, "Email not found, give more specific information");
            }

            var id = emailList[0].Id;

            if (JsonHelper.IsJsonObject(id))
            {
                isSent = await _mailService.ReplyAllMultipleMessages(request, id, token);
            }
            else
            {
                isSent = await _mailService.ReplyAllMessage(id, request.Comment, token);
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ReplyAllMessage] Response:" + isSent);

            if (isSent)
            {
                return Ok("Message sent successfully.");
            }
            else
            {
                return BadRequest("Failed to send message.");
            }
        }

        [HttpPost("forward")]
        public async Task<IActionResult> ForwardMessage(EmailForwardRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ForwardMessage]");
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isSent;

            var findEmailRequest = new QueryEmailsRequest
            {
                From = request.From,
                To = request.To,
                Subject = request.Subject,
                Body = request.Body
            };

            var emailList = await _mailService.ListMessage(findEmailRequest, token);

            if (emailList.Count == 0 || emailList.Count > 1)
            {
                return StatusCode(500, "Email not found, give more specific information");
            }

            var id = emailList[0].Id;

            if (JsonHelper.IsJsonObject(id))
            {
                isSent = await _mailService.ForwardMultipleMessages(request, id, token);
            }
            else
            {
                isSent = await _mailService.ForwardMessage(id, request.ToRecipients, request.Comment, token);
            }

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][ForwardMessage] Response:" + isSent);

            if (isSent)
            {
                return Ok("Message sent successfully.");
            }
            else
            {
                return BadRequest("Failed to send message.");
            }
        }

        [HttpPost("sendMail")]
        public async Task<IActionResult> SendMail(EmailRequest request)
        {
            System.Diagnostics.Debug.WriteLine("[vertex][Messages][SendMail]");
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isSent = await _mailService.SendMail(request, token);

            System.Diagnostics.Debug.WriteLine("[vertex][Messages][SendMail] Response:" + isSent);

            if (isSent)
            {
                return Ok("Message sent successfully.");
            }
            else
            {
                return BadRequest("Failed to send message.");
            }
        }
    }
}
