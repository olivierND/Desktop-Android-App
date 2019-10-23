using Newtonsoft.Json;
using PolyPaint.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using PolyPaint.DataModels;
using PolyPaint.Utils;
using PolyPaint.Views.Controls;

namespace PolyPaint.ViewModels
{
    public class ChatViewModel : BaseViewModel
    {

        #region Private Properties

        private Chat _control;

        private string _channelId;

        #endregion

        #region Public Properties

        public bool IsLoading { get; set; }

        #endregion

        #region Commands

        public ICommand SendMessageCommand { get; set; }

        #endregion

        #region Constructor
        public ChatViewModel(Chat control)
        {
            _control = control;

            SendMessageCommand = new RelayCommand<object>((o) => SendMessage(), (o) => true);
            SetChannelId();
        }

        #endregion

        #region Functions

        public async Task GetMessages()
        {
            IsLoading = true;

            HttpContent resContent = await MainWindowViewModel.ServerService.GetMessages(_channelId);

            IsLoading = false; 

            if (resContent == null)
            {
                return;
            }

            List<Message> messages = await resContent.ReadAsAsync(typeof(List<Message>)) as List<Message>;

            messages?.Reverse();

            //DisplayMessages(ConvertMessagesTimestamps(messages));
        }

        public void DisplayLastMessageReceived(string msg)
        {
            Message message = JsonConvert.DeserializeObject<Message>(msg);
            //message.timestamp = ConvertTimeStampToDateTime(message.timestamp);
            DisplaySingleMessage(message);
        }

        public async void SendMessage()
        {
            string input = _control.InputTextBox.Text;
            _control.InputTextBox.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(input) || IsLoading)
            {
                return;
            }

            await MainWindowViewModel.SignalRService.SendMessage(MainWindowViewModel.Username, _channelId, input.Trim());
        }

        private void SetChannelId()
        {
            if (MainWindowViewModel.CurrentPage == ApplicationPage.WaitingRoomPage)
            {
                _channelId = ((WaitingRoomPageViewModel)MainWindowViewModel.Window.MainFrame.CurrentPage.DataContext).WaitingRoom.Channel.id;
            }
        }

        private void DisplayMessages(List<Message> messages)
        {
            foreach (Message msg in messages)
            {
                DisplaySingleMessage(msg);
            }
        }

        private void DisplaySingleMessage(Message message)
        {
            if (message.senderUsername.Equals(MainWindowViewModel.Username))
            {
                _control.DisplaySentMessage(message);
            }
            else
            {
                _control.DisplayReceivedMessage(message);
            }
        }

        //private List<Message> ConvertMessagesTimestamps(List<Message> messages)
        //{
        //    foreach (Message msg in messages)
        //    {
        //        msg.timestamp = ConvertTimeStampToDateTime(msg.timestamp + "Z");
        //    }

        //    return messages;
        //}

        private string ConvertTimeStampToDateTime(string timestamp)
        {
            return TimeZoneInfo.ConvertTimeBySystemTimeZoneId(DateTime.Parse(timestamp), "Central Standard Time")
                .AddHours(1).ToString("T", new CultureInfo("fr-FR"));
        }
    }

    #endregion
}