using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using game_market_API.Models;
using game_market_API.Services;
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
        [Authorize(Roles = Models.User.AdminRole)]
        [HttpGet]
        public async Task<IEnumerable<GameKey>> GetGameKeys()
        {
            var data = await _gameKeyService.GetGameKeysAsync();
            return data;
        }

        [Authorize(Roles = Models.User.AdminRole + "," + Models.User.VendorRole)]
        // GET: api/GameKey/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameKey>> GetGameKey(int id)
        {
            var game = await _gameKeyService.GetGameKeyAsync(id);
            return game;
        }
        
        [Authorize(Roles = Models.User.VendorRole)]
        // PUT: api/GameKey/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGameKey(int id, GameKey gameKey)
        {
            //TODO: Validation
            if (id != gameKey.ID)
            {
                return BadRequest();
            }

            await _gameKeyService.PutGameKeyAsync(id, gameKey);
            return NoContent();
        }

        [Authorize(Roles = Models.User.VendorRole)]
        // POST: api/GameKey
        [HttpPost]
        public async Task<ActionResult<Game>> PostGameKey(GameKey gameKey)
        {
            //TODO: Validation
            await _gameKeyService.PostGameKeyAsync(gameKey);
            return CreatedAtAction("GetGameKey", new {id = gameKey.ID}, gameKey);
        }

        [Authorize(Roles = Models.User.VendorRole)]
        // DELETE: api/GameKey/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GameKey>> DeleteGame(int id)
        {
            var gameKey = await _gameKeyService.DeleteGameKey(id);
            return gameKey;
        }
    }
}
