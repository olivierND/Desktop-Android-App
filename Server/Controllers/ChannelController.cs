using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Database.Models;

namespace Server.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelController : Controller
    {
        private readonly Projet3DbContext _context;

        public ChannelController(Projet3DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Use user id as user identifier for database entries
        /// </summary>
        /// <returns></returns>
        [ApiExplorerSettings(IgnoreApi = true)]
        public string GetUserId()
        {
            // The user's ID is available in the NameIdentifier claim
            return User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
        }

        // GET: api/channel
        /// <summary>
        /// Returns all channels
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<Channel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<IEnumerable<Channel>> GetChannels()
        {
            return Ok(_context.Channel);
        }


        // GET: api/channel/chat
        /// <summary>
        /// Returns all channels for chat purposes.
        /// </summary>
        /// <returns></returns>
        [HttpGet("chat")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<Channel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<IEnumerable<Channel>> GetChatChannels()
        {
            return Ok(_context.Channel.Where(channel => !channel.IsLobby));
        }

        // GET: api/channel/lobby
        /// <summary>
        /// Return all channels for game lobby purposes
        /// </summary>
        /// <returns></returns>
        [HttpGet("lobby")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<Channel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public ActionResult<IEnumerable<Channel>> GetLobbyChannels()
        {
            return Ok(_context.Channel.Where(channel => channel.IsLobby));
        }

        // GET: api/channel/me
        /// <summary>
        /// Return all channels the user is part of
        /// </summary>
        /// <returns></returns>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<Channel>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<Channel>>> GetUserChannels()
        {
            await _context.UserChannel.LoadAsync();

            var userId = GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            return Ok(_context.Channel
                .Where(channel => channel.UserChannel
                    .Any(userChannel => userChannel.UserId == userId)));
        }

        // GET: api/channel/:channelId/users
        /// <summary>
        /// Return all user ids a channel contains
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        [HttpGet("{channelId}/users")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<Channel>>> GetChannelUsers([FromRoute] string channelId)
        {
            await _context.UserChannel.LoadAsync();

            var queriedChannel = _context.Channel.FirstOrDefault(channel => channel.Id == channelId);
            if (queriedChannel == null)
            {
                return NotFound();
            }

            return Ok(queriedChannel.UserChannel.Select((channel => channel.UserId)));
        }

        // GET: api/message/:channelId/messages
        /// <summary>
        /// Return all messages of a specific channel
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        [HttpGet("{channelId}/messages")]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<Message>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<Message>>> GetChannelMessages([FromRoute] string channelId)
        {
            await _context.Message.LoadAsync();

            var queriedChannel = _context.Channel.FirstOrDefault(channel => channel.Id == channelId);
            if (queriedChannel == null)
            {
                return NotFound();
            }

            return Ok(queriedChannel.Message.OrderByDescending(message => message.Timestamp));
        }

        // POST: api/channel
        /// <summary>
        /// Create a channel
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> CreateChannel([FromBody] Channel channel)
        {
            var newChannel = await _context.Channel.AddAsync(channel);
            await _context.SaveChangesAsync();

            return Created(nameof(GetChannels), newChannel.Entity);
        }

        // DELETE: api/channel/:channelId
        /// <summary>
        /// Delete a channel by id
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        [HttpDelete("{channelId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteChannel([FromRoute] string channelId)
        {
            var queriedChannel = _context.Channel.FirstOrDefault(channel => channel.Id == channelId);
            if (queriedChannel == null)
            {
                return NotFound();
            }

            _context.Channel.Remove(queriedChannel);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}