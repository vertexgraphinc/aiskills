using Microsoft.AspNetCore.Mvc;
using Outlook.Contracts;
using Outlook.Helpers;
using Outlook.Interfaces;
using Outlook.Services;
using System.Linq;
using System.Threading.Tasks;

namespace Outlook.Controllers
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

            bool isSent = await _mailService.DeleteMessage(request, token);
            if (isSent)
            {
                return Ok("Message sent successfully.");
            }
            else
            {
                return BadRequest("Failed to send message.");
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

            bool isSent = await _mailService.ReplyMessage(request, token);
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

            bool isSent = await _mailService.ReplyAllMessage(request, token);
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

            bool isSent = await _mailService.ForwardMessage(request, token);
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
