using System.Threading.Tasks;
using game_market_API.Models;
using game_market_API.Services.ClientService;
using Microsoft.AspNetCore.Mvc;

namespace game_market_API.Controllers
{
    [Route("api/[controller]")]
    public class UserController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromBody]User credentials)
        {
            //TODO: Validation
            var data = await _userService.PostUser(credentials);
            return data;
        }
        
        // GET: api/User/token
        [HttpGet("Token")]
        public async Task<string> GetToken([FromBody]User credentials)
        {
            var data = await _userService.GetToken(credentials);
            return data;
        }
        
    }
}