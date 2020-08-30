using System.Collections.Generic;
using System.Threading.Tasks;
using game_market_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game_market_API.Services
{
    public interface IGameKeyService
    {
        public Task<IEnumerable<GameKey>> GetGameKeysAsync();

        public Task<GameKey> GetGameKeyAsync(int id);

        public Task PutGameKeyAsync(int id, GameKey gameKey);
        
        public  Task<GameKey> PostGameKeyAsync(GameKey gameKey);

        public Task<ActionResult<GameKey>> DeleteGameKey(int id);
    }
}