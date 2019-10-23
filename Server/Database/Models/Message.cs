using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Database.Models
{
    public partial class Message
    {
        [Required]
        public string SenderId { get; set; }
        public string SenderUsername { get; set; }
        [Required]
        public string ChannelId { get; set; }
        public DateTime Timestamp { get; set; }
        [Required]
        public string Content { get; set; }

        public virtual Channel ChannelNavigation { get; set; }
    }
}
