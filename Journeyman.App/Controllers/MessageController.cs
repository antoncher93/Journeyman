using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Journeyman.App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MessageController : ControllerBase
    {
        private readonly IBot _bot;

        public MessageController(IBot bot)
        {
            _bot = bot;
        }

        [HttpGet("test")]
        public string Test()
        {
            return "OK";
        }

        [HttpPost("update")]
        public async Task<ActionResult> Update([FromBody] Update update)
        {
            await _bot.HandleAsync(update);
            return Ok();
        }
    }
}
