using System.Collections.Generic;
using System.Threading.Tasks;
using game_market_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game_market_API.Services
{
    public interface IGameService
    {
        public Task<IEnumerable<Game>> GetGamesAsync();

        public Task<Game> GetGameAsync(int id);

        public Task PutGameAsync(int id, Game game);
        
        public  Task<Game> PostGameAsync(Game game);

        public Task<ActionResult<Game>> DeleteGame(int id);
    }
}