using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using PolyPaint.Models;
using PolyPaint.ViewModels;

namespace PolyPaint.Views.Controls
{
    /// <summary>
    /// Interaction logic for Chat.xaml
    /// </summary>
    public partial class Chat
    {
        public Chat()
        {
            InitializeComponent();

            DataContext = new ChatViewModel(this);
        }

        public void DisplaySentMessage(Message message)
        {
            TextBox msg = new TextBox
            {
                Text = message.content,
                Style = (Style)FindResource("SentMessageStyle"),
            };
            Label sender = new Label
            {
                Content = message.senderUsername + " " + message.timestamp,
                Style = (Style)FindResource("SenderName"),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            Messages.Children.Add(sender);
            Messages.Children.Add(msg);

            MessagesScrollViewer.ScrollToEnd();
        }

        public void DisplayReceivedMessage(Message message)
        {
            TextBox msg = new TextBox
            {
                Text = message.content,
                Style = (Style)FindResource("ReceivedMessageStyle"),
            };
            Label sender = new Label
            {
                Content = message.senderUsername + " " + message.timestamp,
                Style = (Style)FindResource("SenderName"),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            Messages.Children.Add(sender);
            Messages.Children.Add(msg);

            MessagesScrollViewer.ScrollToEnd();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null) await ((ChatViewModel)DataContext)?.GetMessages();
        }

        private void InputTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                ((ChatViewModel)DataContext)?.SendMessage();
            }
        }
    }
}
