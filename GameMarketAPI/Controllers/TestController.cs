using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace game_market_API.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [HttpPost("Test")]
        public async Task<IActionResult> Test([FromBody] string content)
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var body = reader.ReadToEnd();
                // Do something
            }
            return Ok(content);
        } 
    }
}