using Salesforce.Contracts;
using System.Threading.Tasks;

namespace Salesforce.Interfaces
{
    public interface IAuthService
    {
        Task<OAuthToken> RedeemToken(OAuthTokenPara Para);
    }
}
