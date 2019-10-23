using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ClientLourd.Models;
using ClientLourd.ViewModels;
using IdentityModel.OidcClient.Browser;

namespace ClientLourd.Views
{
    /// <summary>
    /// Interaction logic for MessagingClientView.xaml
    /// </summary>
    public partial class MessagingClientView
    {
        private INotifyPropertyChanged _previous;
        private readonly string _userName;

        private List<MessageModel> _messages;
        //private List<string> _onlinePersons;

        private static MessagingClientView _instance;
        private static MessageViewModel _messageViewModel;

        private bool _isMenuVisible;
        private bool _isBlackTheme;

        public MessagingClientView(string userName, string bearerToken)
        {
            InitializeComponent();
            _userName = userName;
            UserName.Text = _userName;

            _messageViewModel = MessageViewModel.GetInstance(bearerToken);
            DataContext = _messageViewModel;

            _previous = (INotifyPropertyChanged) DataContext;
            DataContextChanged += (sender, args) => SubscribeToLastMessageReceivedChanges((INotifyPropertyChanged)args.NewValue);
            SubscribeToLastMessageReceivedChanges(_previous);

            //_onlinePersons = new List<string> { "Olicool", "Chris" };

            //DisplayOnlinePersons();
        }

        public static MessagingClientView GetInstance(string userName, string bearerToken)
        {
            return _instance ?? (_instance = new MessagingClientView(userName, bearerToken));
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            Application.Current.Shutdown();
        }

        private void ChangeTheme_OnClick(object sender, RoutedEventArgs e)
        {
            var bc = new BrushConverter();

            if (!_isBlackTheme)
            {
                ApplicationTitle.Background = Brushes.DimGray;
                Header.Background = Brushes.DimGray;
                LeftPanel.Background = Brushes.DarkGray;
                ConnectedAsPanel.Background = Brushes.DarkGray;
                UserName.Background = Brushes.DarkGray;
                Window.Background = Brushes.Black;
                LeftPanelSeparator.Background = Brushes.LightGray;

                Themes.Content = Properties.Resources.DefaultTheme;
                _isBlackTheme = true;
            }
            else
            {
                ApplicationTitle.Background = (Brush)bc.ConvertFrom("#FF008577");
                Header.Background = (Brush)bc.ConvertFrom("#FF008577");
                LeftPanel.Background = Brushes.LightGray;
                UserName.Background = Brushes.LightGray;
                ConnectedAsPanel.Background = Brushes.LightGray;
                Window.Background = Brushes.White;
                LeftPanelSeparator.Background = Brushes.DarkGray;

                Themes.Content = Properties.Resources.DarkTheme;
                _isBlackTheme = false;
            }
        }

        //private void DisplayOnlinePersons()
        //{
        //    foreach (var gb in _onlinePersons.Select(p => new Label { Content = p, Style = (Style)FindResource("Online") }))
        //    {
        //        OnlinePersons.Children.Add(gb);
        //    }
        //}

        private void SubscribeToLastMessageReceivedChanges(INotifyPropertyChanged viewModel)
        {
            if (_previous != null)
                _previous.PropertyChanged -= LastMessageReceivedChanged;
            _previous = viewModel;
            if (viewModel != null)
                viewModel.PropertyChanged += LastMessageReceivedChanged;
        }

        private void LastMessageReceivedChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName.Equals("LastMessageReceived"))
            {
                if (Application.Current.Dispatcher != null)
                    Application.Current.Dispatcher.Invoke(delegate
                    {
                        DisplaySingleMessage(_messageViewModel.LastMessageReceived);
                    });
            }
        }

        private void DisplayMessages()
        {
            foreach (MessageModel msg in _messages)
            {
                DisplaySingleMessage(msg);
            }
        }

        private void DisplaySingleMessage(MessageModel message)
        {
            if (message.senderusername.Equals(_userName))
            {
                DisplaySentMessage(message);
            }
            else
            {
                DisplayReceivedMessage(message);
            }
        }

        private void DisplaySentMessage(MessageModel message)
        {
            TextBox msg = new TextBox
            {
                Text = message.content,
                Style = (Style)FindResource("SentMessageStyle"),
            };
            Label sender = new Label
            {
                Content = message.senderusername + " " + message.timestamp,
                Style = (Style) FindResource("SenderName"),
                HorizontalAlignment = HorizontalAlignment.Right
            };

            Messages.Children.Add(sender);
            Messages.Children.Add(msg);

            MessagesScrollViewer.ScrollToEnd();
        }

        private void DisplayReceivedMessage(MessageModel message)
        {
            TextBox msg = new TextBox
            {
                Text = message.content,
                Style = (Style)FindResource("ReceivedMessageStyle"),
            };
            Label sender = new Label
            {
                Content = message.senderusername + " " + message.timestamp,
                Style = (Style) FindResource("SenderName"),
                HorizontalAlignment = HorizontalAlignment.Left
            };

            Messages.Children.Add(sender);
            Messages.Children.Add(msg);

            MessagesScrollViewer.ScrollToEnd();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _messageViewModel.IsLoading = true;

            await _messageViewModel.GetMessages().ContinueWith((res) =>
            {
                _messages = res.Result;
                _messageViewModel.IsLoading = false;
            });

            if (_messages != null)
            {
                DisplayMessages();
            }
            else
            {
                MessageBox.Show(Properties.Resources.MessageLoadingError);
            }
        }

        private void InputTextBox_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                SendMessage_OnClick(sender, e);
            }
        }

        private async void SendMessage_OnClick(object sender, RoutedEventArgs e)
        {
            string input = InputTextBox.Text;
            InputTextBox.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(input))
            {
                return;
            }

            string res = await _messageViewModel.SendMessage(_userName, input.Trim());

            if (res != null)
            {
                MessageBox.Show(res);
            }
        }

        private void Dropdown_OnMouseUp(object sender, RoutedEventArgs e)
        {
            if (_isMenuVisible)
            {
                _isMenuVisible = false;
                Menu.Visibility = Visibility.Hidden;
            }
            else
            {
                _isMenuVisible = true;
                Menu.Visibility = Visibility.Visible;
            }
        }

        private async void Logout_OnClick(object sender, RoutedEventArgs e)
        {
            if (_isMenuVisible)
            {
                _isMenuVisible = false;
                Menu.Visibility = Visibility.Hidden;
            }

            Hide();
            BrowserResultType browserResult = await AuthenticationView.Client.LogoutAsync();

            if (browserResult == BrowserResultType.Success)
            {
                AuthenticationView authenticationView = new AuthenticationView();
                authenticationView.Show();
            }
            else
            {
                MessageBox.Show(Properties.Resources.LogoutErrorMessage);
            }
        }

        private void Logout_OnMouseEnter(object sender, RoutedEventArgs e)
        {
            Logout.Background = Brushes.LightGray;
        }

        private void Logout_OnMouseLeave(object sender, RoutedEventArgs e)
        {
            Logout.Background = Brushes.DarkGray;
        }

        private void ChangeTheme_OnMouseEnter(object sender, RoutedEventArgs e)
        {
            Themes.Background = Brushes.LightGray;
        }

        private void ChangeTheme_OnMouseLeave(object sender, RoutedEventArgs e)
        {
            Themes.Background = Brushes.DarkGray;
        }
    }
}
