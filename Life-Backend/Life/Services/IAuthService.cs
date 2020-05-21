using System.Threading.Tasks;
using Life.Models;

namespace Life.Services
{
    public interface IAuthService
    {
        Task<User> Authenticate(Google.Apis.Auth.GoogleJsonWebSignature.Payload payload);
        Task<User> Authenticate(string username, string password);
    }
}