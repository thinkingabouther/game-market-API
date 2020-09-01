using System.Collections.Generic;
using System.Threading.Tasks;
using game_market_API.DTOs;
using game_market_API.Models;
using game_market_API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game_market_API.Services
{
    public interface IGameService
    {
        public Task<IEnumerable<GameViewModel>> GetGamesAsync();

        public Task<GameViewModel> GetGameAsync(int id);

        public Task PatchGameAsync(string vendorUserName, int id, GameDto game);
        
        public  Task<GameViewModel> PostGameAsync(string vendorUserName, Game game);

        public Task<GameViewModel> DeleteGame(string vendorUserName, int id);
    }
}