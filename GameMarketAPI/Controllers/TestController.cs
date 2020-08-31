using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace game_market_API.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [HttpPost("Test")]
        public async Task<IActionResult> Test()
        {
            return Ok();
        } 
    }
}