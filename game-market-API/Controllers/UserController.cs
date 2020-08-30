using System.Threading.Tasks;
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
        // POST: api/User/Vendor
        [HttpPost("Vendor")]
        public async Task<ActionResult<User>> PostVendor([FromBody]User credentials)
        {
            //TODO: Validation
            credentials.Role = Models.User.VendorRole;
            var data = await _userService.PostUser(credentials);
            return data;
        }
        
        // POST: api/User/Client
        [HttpPost("Client")]
        public async Task<ActionResult<User>> PostClient([FromBody]User credentials)
        {
            //TODO: Validation
            credentials.Role = Models.User.ClientRole;
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