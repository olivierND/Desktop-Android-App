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

        private static Message BuildNotification(string channelId, string message)
        {
            return new Message
            {
                ChannelId = channelId,
                Content = message,
                Timestamp = DateTime.UtcNow
            };
        }

        /// <summary>
        /// Adds user to a specific channel and notifies all other channel members
        /// </summary>
        /// <param name="username"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public async Task Join(string username, string channelId)
        {
            var userId = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var queriedChannel = await _context.Channel.FindAsync(channelId);
            if (string.IsNullOrEmpty(userId) || queriedChannel == null)
            {
                throw new HubException("Invalid user or channel id provided.");
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, channelId);

            // If user first time joining, notify other chat members and save to the db
            var queriedUserChannel = await _context.UserChannel.FindAsync(userId, channelId);
            if (queriedUserChannel == null)
            {
                await Clients.Group(channelId).SendAsync("Join", BuildNotification(channelId, $"{username} à rejoint le canal."));

                var newUserChannel = new UserChannel
                {
                    UserId = userId,
                    ChannelId = channelId
                };

                await _context.UserChannel.AddAsync(newUserChannel);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Removes user from a specific channel and notifies all other channel members
        /// </summary>
        /// <param name="username"></param>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public async Task Leave(string username, string channelId)
        {
            var userId = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var queriedChannel = await _context.Channel.FindAsync(channelId);
            if (string.IsNullOrEmpty(userId) || queriedChannel == null)
            {
                throw new HubException("Invalid user or channel id provided.");
            }

            await Groups.RemoveFromGroupAsync(Context.ConnectionId, channelId);

            // If user was indeed part of the channel, remove him and notify other members
            var queriedUserChannel = await _context.UserChannel.FindAsync(userId, channelId);
            if (queriedUserChannel != null)
            {
                await Clients.Group(channelId).SendAsync("Leave", BuildNotification(channelId, $"{username} à quitté le canal."));

                _context.UserChannel.Remove(queriedUserChannel);
                await _context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Receive message from client, save it to the db, then forward it to all other clients
        /// </summary>
        /// <param name="username"></param>
        /// <param name="channelId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendMessage(string username, string channelId, string message)
        {
            var senderId = Context.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var queriedChannel = await _context.Channel.FindAsync(channelId);
            if (string.IsNullOrEmpty(senderId) || queriedChannel == null)
            {
                throw new HubException("Invalid user or channel id provided.");
            }

            var queriedUserChannel = await _context.UserChannel.FindAsync(senderId, queriedChannel.Id);
            if (queriedUserChannel == null)
            {
                throw new HubException("User not part of given channel.");
            }

            var newMessage = new Message
            {
                SenderId = senderId,
                SenderUsername = username,
                ChannelId = channelId,
                Content = message,
                Timestamp = DateTime.UtcNow
            };

            await Clients.Group(channelId).SendAsync("ReceiveMessage", JsonConvert.SerializeObject(newMessage, _jsonSerializerSettings));
            await _context.Message.AddAsync(newMessage);
            await _context.SaveChangesAsync();
        }
    }
}