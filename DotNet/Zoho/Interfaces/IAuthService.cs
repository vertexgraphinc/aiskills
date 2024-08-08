using Zoho.Contracts;
using System.Threading.Tasks;

namespace Zoho.Interfaces
{
    public interface IAuthService
    {
        Task<OAuthToken> RedeemToken(OAuthTokenPara Para);
    }
}