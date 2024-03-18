using MSOutlook.Contracts;
using System.Threading.Tasks;

namespace MSOutlook.Interfaces
{
    public interface IAuthService
    {
        Task<OAuthToken> RedeemToken(OAuthTokenPara Para);
    }
}
