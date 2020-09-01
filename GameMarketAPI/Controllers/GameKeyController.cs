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
        /// <summary>Gets all the keys that belong to the vendor with given token</summary>
        /// <param name="JWT">Token of the current user. Starts with "Bearer .."</param>
        /// <response code="200">Returns in case of success</response>
        /// <response code="401">Returns in case the token was not given</response>
        /// <response code="404">Returns in case there are no games that belong to the vendor</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Models.User.VendorRole)]
        [HttpGet]
        public async Task<IEnumerable<GameKeyViewModel>> GetGameKeys([FromHeader(Name = "Authorization")]string JWT)
        {
            var data = await _gameKeyService.GetGameKeysAsync(User.Identity.Name);
            return data;
        }
        
        // GET: api/GameKey/5
        /// <summary>Gets the key with given id that belongs to the vendor with given token</summary>
        /// <param name="JWT">Token of the current user. Starts with "Bearer .."</param>
        /// <response code="200">Returns in case of success</response>
        /// <response code="401">Returns in case the token was not given or the key with given id belongs to another user</response>
        /// <response code="404">Returns in case there is no game key with such id</response>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(Roles = Models.User.VendorRole)]
        
        [HttpGet("{id}")]
        public async Task<ActionResult<GameKeyViewModel>> GetGameKey([FromHeader(Name = "Authorization")]string JWT, int id)
        {
            var game = await _gameKeyService.GetGameKeyAsync(User.Identity.Name, id);
            return game;
        }
        
        /// <summary>Adds a key to the game with given id</summary>
        /// <param name="JWT">Token of the current user. Starts with "Bearer .."</param>
        /// <response code="201">Returns in case of success</response>
        /// <response code="401">Returns in case the token was not given or the game with given id belongs to another user</response>
        /// <response code="404">Returns in case there is no game with such id</response>
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = Models.User.VendorRole)]
        // POST: api/GameKey
        [HttpPost]
        public async Task<ActionResult<GameKeyViewModel>> PostGameKey([FromHeader(Name = "Authorization")]string JWT, GameKeyDto gameKeyDto)
        {
            //TODO: Validation
            var gameKey = await _gameKeyService.PostGameKeyAsync(User.Identity.Name, gameKeyDto);
            return CreatedAtAction("GetGameKey", new {id = gameKey.ID}, gameKey);
        }
        /// <summary>Deletes a key with given id</summary>
        /// <param name="JWT">Token of the current user. Starts with "Bearer .."</param>
        /// <response code="204">Returns in case of success</response>
        /// <response code="401">Returns in case the token was not given or the game key with given id belongs to another user</response>
        /// <response code="404">Returns in case there is no game key with such id</response>
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = Models.User.VendorRole)]
        // DELETE: api/GameKey/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GameKeyViewModel>> DeleteGame([FromHeader(Name = "Authorization")]string JWT, int id)
        {
            var gameKey = await _gameKeyService.DeleteGameKey(User.Identity.Name, id);
            return gameKey;
        }
    }
}
