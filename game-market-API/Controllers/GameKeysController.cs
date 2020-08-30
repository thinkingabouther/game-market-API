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
        [Authorize(Roles = Models.User.AdminRole + "," + Models.User.VendorRole)]
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
        // POST: api/GameKey
        [HttpPost]
        public async Task<ActionResult<GameKey>> PostGameKey(GameKeyRequest gameKeyRequest)
        {
            //TODO: Validation
            var gameKey = await _gameKeyService.PostGameKeyAsync(User.Identity.Name, gameKeyRequest);
            return CreatedAtAction("GetGameKey", new {id = gameKey.ID}, gameKey);
        }

        [Authorize(Roles = Models.User.VendorRole)]
        // DELETE: api/GameKey/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GameKey>> DeleteGame(int id)
        {
            var gameKey = await _gameKeyService.DeleteGameKey(User.Identity.Name, id);
            return gameKey;
        }


    }
}
