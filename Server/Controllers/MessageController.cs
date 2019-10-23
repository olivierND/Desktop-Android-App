using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Database;
using Server.Database.Models;

namespace Server.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : Controller
    {
        private readonly Projet3DbContext _context;

        public MessageController(Projet3DbContext context)
        {
            _context = context;
        }

        // GET: api/message
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<Message>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<IEnumerable<Message>> GetMessages([FromQuery] int page, [FromQuery] int size)
        {
            // Page 0 = first page (not skipping any message)
            return size == 0
                ? Ok(_context.Message.OrderByDescending(message => message.Timestamp))
                : Ok(_context.Message.OrderByDescending(message => message.Timestamp).Skip(page * size).Take(size));
        }

        // POST: api/message
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> AddMessage([FromBody] Message message)
        {
            var newMessage = await _context.Message.AddAsync(message);
            await _context.SaveChangesAsync();

            return Created(nameof(GetMessages), newMessage.Entity);
        }
    }
}