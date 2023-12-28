using Microsoft.Contracts;
using System.Threading.Tasks;

namespace Microsoft.Interfaces
{
    public interface IAuthService
    {
        Task<OAuthToken> RedeemToken(OAuthTokenPara Para);
    }
}
