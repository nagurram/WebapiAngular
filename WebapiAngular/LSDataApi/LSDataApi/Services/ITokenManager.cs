using System.Threading.Tasks;

namespace LSDataApi.Services
{
    public interface ITokenManager
    {
        Task<bool> IsCurrentActiveToken();

        Task SaveToken(int user, string accesstoken, string secret);

        Task DeactivateCurrentAsync();

        Task<bool> IsActiveAsync(string token);

        Task DeactivateAsync(string token);

        Task<bool> Skipvalidation(string path);
    }
}