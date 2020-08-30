using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
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
    public class GameController : Controller
    {
        private readonly IGameService _gameService;
        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        // GET: api/Games
        [HttpGet]
        public async Task<IEnumerable<Game>> GetGames()
        {
            var data = await _gameService.GetGamesAsync();
            return data;
        }

        // GET: api/Game/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int id)
        {
            var game = await _gameService.GetGameAsync(id);
            return game;
        }
        
        [Authorize(Roles = Models.User.AdminRole)]
        // PUT: api/Game/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int id, Game game)
        {
            //TODO: Validation
            if (id != game.ID)
            {
                return BadRequest();
            }

            await _gameService.PutGameAsync(id, game);
            
            return NoContent();
        }
        
        [Authorize(Roles = Models.User.VendorRole)]
        // POST: api/Game
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            //TODO: Validation
            await _gameService.PostGameAsync(game);
            return CreatedAtAction("GetGame", new { id = game.ID }, game);
        }
        
        [Authorize(Roles = Models.User.VendorRole)]
        // DELETE: api/Game/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> DeleteGame(int id)
        {
            var game = await _gameService.DeleteGame(id);
            return game;
        }
    }
}
