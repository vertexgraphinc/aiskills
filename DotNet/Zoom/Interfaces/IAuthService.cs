using Zoom.Contracts;
using System.Threading.Tasks;

namespace Zoom.Interfaces
{
    public interface IAuthService
    {
        Task<OAuthToken> RedeemToken(OAuthTokenPara Para);
    }
}
