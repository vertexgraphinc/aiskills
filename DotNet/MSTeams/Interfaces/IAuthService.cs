using MSTeams.Contracts;
using System.Threading.Tasks;

namespace MSTeams.Interfaces
{
    public interface IAuthService
    {
        Task<OAuthToken> RedeemToken(OAuthTokenPara Para);
    }
}
