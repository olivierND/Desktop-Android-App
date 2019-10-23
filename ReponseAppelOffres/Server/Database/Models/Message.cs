using System;

namespace Server.Database.Models
{
    public class Message
    {
        public string SenderId { get; set; }
        public string SenderUsername { get; set; }
        public string Channel { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
