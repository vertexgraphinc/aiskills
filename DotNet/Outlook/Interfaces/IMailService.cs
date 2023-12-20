using Outlook.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Outlook.Interfaces
{
    public interface IMailService
    {
        Task<List<EmailResponse>> ListMessage(QueryEmailsRequest request, string token);

        Task<EmailResponse> GetMessage(EmailIdRequest request, string token);

        Task<bool> DeleteMessage(EmailIdRequest request, string token);

        Task<bool> SendDraftMessage(EmailIdRequest request, string token);

        Task<bool> ReplyMessage(EmailReplyRequest request, string token);

        Task<bool> ReplyAllMessage(EmailReplyAllRequest request, string token);

        Task<bool> ForwardMessage(EmailForwardRequest request, string token);

        Task<bool> SendMail(EmailRequest request, string Token);

    }
}
