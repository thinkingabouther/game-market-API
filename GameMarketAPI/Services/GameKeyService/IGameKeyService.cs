using System.Collections.Generic;
using System.Threading.Tasks;
using game_market_API.DTOs;
using game_market_API.Models;
using game_market_API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace game_market_API.Services
{
    public interface IGameKeyService
    {
        public Task<IEnumerable<GameKeyViewModel>> GetGameKeysAsync(string vendorUserName);

        public Task<GameKeyViewModel> GetGameKeyAsync(string vendorUserName, int id);
        
        public  Task<GameKeyViewModel> PostGameKeyAsync(string vendorUserName, GameKeyDto gameKey);

        public Task<ActionResult<GameKeyViewModel>> DeleteGameKey(string vendorUserName, int id);
    }
}