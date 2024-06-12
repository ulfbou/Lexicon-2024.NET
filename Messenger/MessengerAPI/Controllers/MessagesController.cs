using MessengerAPI.Data;
using MessengerAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MessengerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : ControllerBase
    {
        private readonly MessangerContext _context;

        public MessagesController(MessangerContext context)
        {
            _context = context;
        }

        [HttpPost("send")]
        [Authorize]
        public async Task<ActionResult<Message>> SendMessage(Message message)
        {
            message.Timestamp = DateTime.Now;

            _context.Messages.Add(message);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(SendMessage), message);
        }

    }
}
