using System.Threading.Tasks;
using game_market_API.DTOs;
using game_market_API.Models;
using game_market_API.Services.ClientService;
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
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody] UserCredentialsDto userCredentialsDto)
        {
            var data = await _userService.PostUser(userCredentialsDto);
            return data;
        }

        // GET: api/User/token
        [HttpPost("Token")]
        public async Task<IActionResult> GetToken([FromBody]User credentials)
        {
            var data = await _userService.GetToken(credentials);
            return Ok(new
            {
                token = data
            });
        }
        
    }
}