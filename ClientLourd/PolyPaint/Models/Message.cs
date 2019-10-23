using System;

namespace PolyPaint.Models
{
    public class Message
    {
        public string senderId { get; set; }
        public string senderUsername { get; set; }
        public string channelId { get; set; }
        public DateTime timestamp { get; set; }
        public string content { get; set; }
    }
}