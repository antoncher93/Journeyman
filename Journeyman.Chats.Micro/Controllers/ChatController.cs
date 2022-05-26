using Journeyman.Chats.Micro.Context;
using Journeyman.Chats.Micro.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Journeyman.Chats.Micro.Controllers
{
    [ApiController, Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ChatDbContext _db;

        public ChatController(ChatDbContext db)
        {
            _db = db;   
        }

        [HttpGet("{chatId}")]
        public async Task<ActionResult<ChatAgreement>> Get(long chatId)
        {
            var chatAgreement = await _db.ChatAgreements.FirstOrDefaultAsync(c => long.Equals(c.ChatId, chatId));
            if (chatAgreement is null)
                return NotFound();
            return Ok(chatAgreement);
        }
    }
}
