using System.Collections.Generic;

namespace PolyPaint.Models
{
    public class Channel
    {
        public string id { get; set; }
        public string name { get; set; }
        public List<Message> message { get; set; }
        public List<UserChannel> userChannel { get; set; }
        public bool isLobby { get; set; }
    }
}