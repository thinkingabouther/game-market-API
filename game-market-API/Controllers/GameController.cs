using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoWrapper.Wrappers;
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
        
        [Authorize(Roles = Models.User.VendorRole)]
        // PUT: api/Game/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchGame(int id, GameDto game)
        {
            //TODO: Validation

            await _gameService.PatchGameAsync(User.Identity.Name, id, game);
            
            return NoContent();
        }
        
        [Authorize(Roles = Models.User.VendorRole)]
        // POST: api/Game
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(Game game)
        {
            //TODO: Validation
            await _gameService.PostGameAsync(User.Identity.Name, game);
            return CreatedAtAction("GetGame", new { id = game.ID }, game);
        }
        
        [Authorize(Roles = Models.User.VendorRole)]
        // DELETE: api/Game/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> DeleteGame(int id)
        {
            var game = await _gameService.DeleteGame(User.Identity.Name, id);
            return game;
        }
        
        [Authorize(Roles = Models.User.ClientRole)]
        [HttpPost("Purchase")]
        public async Task<ActionResult<PaymentSession>> PreparePaymentSession(PurchaseDto purchaseRequest)
        {
            var session = await _gameService.PreparePaymentSession(User.Identity.Name, purchaseRequest);
            return session;
        }
    }
}
