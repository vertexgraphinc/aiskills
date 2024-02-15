using Microsoft.AspNetCore.Mvc;
using Microsoft.Contracts;
using Microsoft.Helpers;
using Microsoft.Interfaces;
using Microsoft.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Controllers
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
            return resp;
        }

        [HttpPost("get")]
        public async Task<EmailResponse> GetMessage(EmailIdRequest request)
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            return await _mailService.GetMessage(request, token);
        }

        [HttpPost("delete")]
        public async Task<IActionResult> DeleteMessage(EmailIdRequest request)
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isDeleted = await _mailService.DeleteMessage(request, token);
            if (isDeleted)
            {
                return Ok("Message deleted successfully.");
            }
            else
            {
                return BadRequest("Failed to delete message.");
            }
        }

        [HttpPost("sendDraft")]
        public async Task<IActionResult> SendDraftMessage(EmailIdRequest request)
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isSent = await _mailService.SendDraftMessage(request, token);
            if (isSent)
            {
                return Ok("Message sent successfully.");
            }
            else
            {
                return BadRequest("Failed to send message.");
            }
        }

        [HttpPost("reply")]
        public async Task<IActionResult> ReplyMessage(EmailReplyRequest request)
        {
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isSent;

            if (JsonHelper.IsJsonObject(request.Id))
            {
                isSent = await _mailService.ReplyMultipleMessages(request, token);
            } else
            {
                isSent = await _mailService.ReplyMessage(request.Id, request.ToRecipients, request.Comment, token);
            }

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
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isSent;

            if (JsonHelper.IsJsonObject(request.Id))
            {
                isSent = await _mailService.ReplyAllMultipleMessages(request, token);
            }
            else
            {
                isSent = await _mailService.ReplyAllMessage(request.Id, request.Comment, token);
            }

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
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isSent;

            if (JsonHelper.IsJsonObject(request.Id))
            {
                isSent = await _mailService.ForwardMultipleMessages(request, token);
            }
            else
            {
                isSent = await _mailService.ForwardMessage(request.Id, request.ToRecipients, request.Comment, token);
            }

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
            string authorizationHeader = Request.Headers["Authorization"].FirstOrDefault();
            string token = TokenHelper.GetSessionToken(authorizationHeader);
            if (string.IsNullOrEmpty(token))
            {
                Response.StatusCode = 401;
                return null;
            }

            bool isSent = await _mailService.SendMail(request, token);
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
