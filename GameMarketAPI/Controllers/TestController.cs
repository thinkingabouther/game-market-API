using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace game_market_API.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Test([FromBody] string content)
        {
            using (var reader = new StreamReader(Request.Body))
            {
                var body = reader.ReadToEnd();
                return Ok(body);
            }
        } 
    }
}