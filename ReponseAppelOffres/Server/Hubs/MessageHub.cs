using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Server.Database;
using Server.Database.Models;

namespace Server.Hubs
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly string _groupName = "Chat Users";
        private readonly Projet3DbContext _context;
        private readonly JsonSerializerSettings _jsonSerializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            }
        };

        public MessageHub(Projet3DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Used to notify others of user joining chat
        /// </summary>
        /// <returns></returns>
        public override async Task OnConnectedAsync()
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, _groupName);
            await base.OnConnectedAsync();

            await Clients.All.SendAsync("Connected", "Someone has joined the channel.");
        }

        /// <summary>
        /// Used to notify others of user leaving chat
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, _groupName);
            await base.OnDisconnectedAsync(exception);

            await Clients.All.SendAsync("Disconnected", "Someone has left the channel.");
        }

        /// <summary>
        /// Receive message from client, save it to the db, then forward it to all other clients
        /// </summary>
        /// <param name="username"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string username, string message)
        {
            var senderId = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(senderId) && !string.IsNullOrEmpty(message))
            {
                var newMessage = new Message
                {
                    SenderId = senderId,
                    SenderUsername = username,
                    Channel = _groupName,
                    Content = message,
                    Timestamp = DateTime.UtcNow
                };

                await Clients.All.SendAsync("ReceiveMessage", JsonConvert.SerializeObject(newMessage, _jsonSerializerSettings));
                await _context.Message.AddAsync(newMessage);
                await _context.SaveChangesAsync();
            }
        }
    }
}
