using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Database.Models
{
    public partial class Channel
    {
        public Channel()
        {
            Message = new HashSet<Message>();
            UserChannel = new HashSet<UserChannel>();
        }

        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        public bool IsLobby { get; set; }

        public virtual ICollection<Message> Message { get; set; }
        public virtual ICollection<UserChannel> UserChannel { get; set; }
    }
}
