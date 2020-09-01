using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using game_market_API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using game_market_API.Models;
using game_market_API.Services;
using game_market_API.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace game_market_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameKeyController : Controller
    {
        private readonly IGameKeyService _gameKeyService;
        public GameKeyController(IGameKeyService gameKeyService)
        {
            _gameKeyService = gameKeyService;
        }

        // GET: api/GameKey
        [Authorize(Roles = Models.User.VendorRole)]
        [HttpGet]
        public async Task<IEnumerable<GameKeyViewModel>> GetGameKeys()
        {
            var data = await _gameKeyService.GetGameKeysAsync(User.Identity.Name);
            return data;
        }

        [Authorize(Roles = Models.User.VendorRole)]
        // GET: api/GameKey/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameKeyViewModel>> GetGameKey(int id)
        {
            var game = await _gameKeyService.GetGameKeyAsync(User.Identity.Name, id);
            return game;
        }
        

        [Authorize(Roles = Models.User.VendorRole)]
        // POST: api/GameKey
        [HttpPost]
        public async Task<ActionResult<GameKeyViewModel>> PostGameKey(GameKeyDto gameKeyDto)
        {
            //TODO: Validation
            var gameKey = await _gameKeyService.PostGameKeyAsync(User.Identity.Name, gameKeyDto);
            return CreatedAtAction("GetGameKey", new {id = gameKey.ID}, gameKey);
        }

        [Authorize(Roles = Models.User.VendorRole)]
        // DELETE: api/GameKey/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GameKeyViewModel>> DeleteGame(int id)
        {
            var gameKey = await _gameKeyService.DeleteGameKey(User.Identity.Name, id);
            return gameKey;
        }
    }
}
