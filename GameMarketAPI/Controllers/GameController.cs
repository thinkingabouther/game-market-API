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
    public class GameController : Controller
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }
        /// <summary>
        /// Gets all available games
        /// </summary>
        /// <returns>Returns an array with all games</returns>
        /// <response code="200">Returns in case of success</response>
        /// <response code="404">Returns in case there are no games in the store</response>
        // GET: api/Games
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IEnumerable<GameViewModel>> GetGames()
        {
            var data = await _gameService.GetGamesAsync();
            return data;
        }
        
        /// <summary>
        /// Gets particular game by id
        /// </summary>
        /// <returns>Returns game model with given id</returns>
        /// <response code="200">Returns in case of  success</response>
        /// <response code="404">Returns in case there is no game with such id</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        // GET: api/Game/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GameViewModel>> GetGame(int id)
        {
            var game = await _gameService.GetGameAsync(id);
            return game;
        }
        
        /// <summary>
        /// Patches particular game by id
        /// </summary>
        /// <param name="game">Game model</param>
        /// <param name="JWT">Token of the current user. Starts with "Bearer .."</param>
        /// <returns>Returns game model with given id</returns>
        /// <response code="204">Returns in case of success</response>
        /// <response code="401">Returns in case the token was not given or the game with given id was created by another vendor</response>
        /// <response code="404">Returns in case there is no game with such id</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = Models.User.VendorRole)]
        // PUT: api/Game/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchGame([FromHeader(Name = "Authorization")]string JWT, int id, GameDto game)
        {
            
            await _gameService.PatchGameAsync(User.Identity.Name, id, game);
            return NoContent();
        }
        
        /// <summary>
        /// Adds a new game
        /// </summary>
        /// <param name="game">Game model</param> 
        /// <param name="JWT">Token of the current user. Starts with "Bearer .."</param>
        /// <response code="201">Returns in case of success</response>
        /// <response code="400">Returns in case something went wrong during the request. See message for details</response>
        /// <response code="401">Returns in case vendor token was not given</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = Models.User.VendorRole)]
        // POST: api/Game
        [HttpPost]
        public async Task<ActionResult<GameViewModel>> PostGame([FromHeader(Name = "Authorization")]string JWT, Game game)
        {
            await _gameService.PostGameAsync(User.Identity.Name, game);
            return CreatedAtAction("GetGame", new { id = game.ID }, game);
        }
        
        /// <summary>
        /// Deletes particular game by id
        /// </summary>
        /// <param name="JWT">Token of the current user. Starts with "Bearer .."</param>
        /// <returns>Returns the deleted game model</returns>
        /// <response code="200">Returns in case of success</response>
        /// <response code="401">Returns in case the token was not given or the game with given id was created by another vendor</response>
        /// <response code="404">Returns in case there is no game with such id</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Models.User.VendorRole)]
        // DELETE: api/Game/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GameViewModel>> DeleteGame([FromHeader(Name = "Authorization")]string JWT, int id)
        {
            var game = await _gameService.DeleteGame(User.Identity.Name, id);
            return game;
        }
    }
}
