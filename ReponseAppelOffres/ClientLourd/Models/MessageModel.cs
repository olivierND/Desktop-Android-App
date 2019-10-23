using System;

namespace ClientLourd.Models
{
    public class MessageModel
    {
        public string content { get; set; }
        public string senderid { get; set; }
        public string timestamp { get; set; }
        public string senderusername { get; set; }
        public string channel { get; set; }
    }
}