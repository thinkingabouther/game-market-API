using System.Threading.Tasks;
using game_market_API.Models;

namespace game_market_API.Services.ClientService
{
    public interface IUserService
    {
        public Task<User> PostUser(User credentials);

        public Task<string> GetToken(User credentials);
    }
}