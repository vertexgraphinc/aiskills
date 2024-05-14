using Jira.Contracts;
using System.Threading.Tasks;

namespace Jira.Interfaces
{
    public interface IAuthService
    {
        Task<OAuthToken> RedeemToken(OAuthTokenPara Para);
    }
}
