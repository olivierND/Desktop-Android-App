using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Server.Database.Models
{
    public partial class UserChannel
    {
        [Required]
        public string UserId { get; set; }
        [Required]
        public string ChannelId { get; set; }

        public virtual Channel Channel { get; set; }
    }
}
