using System.Threading.Tasks;
using game_market_API.DTOs;
using game_market_API.Models;
using game_market_API.Services.ClientService;
using Microsoft.AspNetCore.Mvc;

namespace game_market_API.Controllers
{
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
        public async Task<ActionResult<User>> PostUser(UserCredentialsDto userCredentialsDto)
        {
            var data = await _userService.PostUser(userCredentialsDto);
            return data;
        }

        // GET: api/User/token
        [HttpPost("Token")]
        public async Task<string> GetToken([FromBody]User credentials)
        {
            var data = await _userService.GetToken(credentials);
            return data;
        }
        
    }
}