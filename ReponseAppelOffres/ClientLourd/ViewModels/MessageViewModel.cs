using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ClientLourd.Models;
using ClientLourd.Services;
using Newtonsoft.Json;

namespace ClientLourd.ViewModels
{
    public class MessageViewModel : INotifyPropertyChanged
    {
        private readonly ServerService _serverService;
        private readonly SignalRService _signalRService;
        private static MessageViewModel _instance;
        private MessageModel _lastMessageReceived;

        private bool _isLoading;

        public event PropertyChangedEventHandler PropertyChanged;

        public MessageModel LastMessageReceived
        {
            get => _lastMessageReceived;
            set
            {
                _lastMessageReceived = value;
                OnPropertyChanged("LastMessageReceived");
            }
        }

        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged("IsLoading");
            }
        }

        public MessageViewModel(string bearerToken)
        {
            _serverService = new ServerService(bearerToken);
            _signalRService = new SignalRService(bearerToken);
        }

        public static MessageViewModel GetInstance(string bearerToken)
        {
            return _instance ?? (_instance = new MessageViewModel(bearerToken));
        }

        public async Task<List<MessageModel>> GetMessages()
        {
            HttpContent res = await _serverService.GetMessages();

            if (res == null)
            {
                return null;
            }

            List<MessageModel> messages = await res.ReadAsAsync(typeof(List<MessageModel>)) as List<MessageModel>;
            messages?.Reverse();
            return ConvertMessagesTimestamps(messages);
        }

        public void SetLastMessageReceived(string msg)
        {
            MessageModel message = JsonConvert.DeserializeObject<MessageModel>(msg);
            message.timestamp = ConvertTimeStampToDateTime(message.timestamp);
            LastMessageReceived = message;
        }

        public async Task<string> SendMessage(string username, string message)
        {
            return await _signalRService.SendMessage(username,message);
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private List<MessageModel> ConvertMessagesTimestamps(List<MessageModel> messages)
        {
            foreach (MessageModel msg in messages)
            {
                msg.timestamp = ConvertTimeStampToDateTime(msg.timestamp + "Z");
            }

            return messages;
        }

        private string ConvertTimeStampToDateTime(string timestamp)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(timestamp), "Central Standard Time")
                .AddHours(1).ToString("T", new CultureInfo("fr-FR"));
        }
    }
}