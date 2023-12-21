using Outlook.Contracts;
using System.Threading.Tasks;

namespace Outlook.Interfaces
{
    public interface IAuthService
    {
        Task<OAuthToken> RedeemToken(OAuthTokenPara Para);
    }
}
