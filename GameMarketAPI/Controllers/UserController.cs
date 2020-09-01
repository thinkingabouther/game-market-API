using System.Threading.Tasks;
using game_market_API.DTOs;
using game_market_API.Models;
using game_market_API.Services.ClientService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace game_market_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        // POST: api/User/User
        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="userCredentialsDto">change IsVendor flag to create client or vendor</param>
        /// <response code="200">Returns in case of success</response>
        /// <response code="400">Returns in case something went wrong. Check message for details</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<User>> PostUser([FromBody] UserCredentialsDto userCredentialsDto)
        {
            var data = await _userService.PostUser(userCredentialsDto);
            return data;
        }
        /// <summary>
        /// Gets a token for a user with given credentials
        /// </summary>
        /// <param name="credentials"></param>
        /// <response code="200">Returns in case of success</response>
        /// <response code="404">Returns in case there is no such user registered</response>
        // GET: api/User/token
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPost("Token")]
        public async Task<IActionResult> GetToken([FromBody]UserCredentialsForTokenDto credentials)
        {
            var data = await _userService.GetToken(credentials);
            return Ok(new
            {
                token = data
            });
        }
        
    }
}