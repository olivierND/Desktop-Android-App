using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Auth0.OidcClient;
using Fasetto.Word;
using GalaSoft.MvvmLight.Messaging;
using IdentityModel.OidcClient.Browser;
using PolyPaint.DataModels;
using PolyPaint.Models;
using PolyPaint.Properties;
using PolyPaint.Services;
using PolyPaint.Utils;
using PolyPaint.Views.Windows;

namespace PolyPaint.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Private Properties

        private int _outerMarginSize = 10;

        private int _windowRadius = 10;

        private string _bearerToken;

        private string _lastMessageUpdated;

        private readonly List<ApplicationPage> _pagesNotInNavigation = new List<ApplicationPage>
        {
            ApplicationPage.ProfilePage,
            ApplicationPage.InviteFriendOrBotPage,
            ApplicationPage.LoadingPage,
        };


        #endregion

        #region Public Properties

        public readonly MainWindow Window;

        public readonly Auth0Client Client = new Auth0Client(new Auth0ClientOptions
        {
            Domain = ConfigurationManager.AppSettings["Auth0:Domain"],
            ClientId = ConfigurationManager.AppSettings["Auth0:ClientId"],
            ClientSecret = ConfigurationManager.AppSettings["Auth0:ClientSecret"]
        });

        public ApplicationPage CurrentPage { get; set; } = ApplicationPage.WelcomePage;

        public ApplicationPage PreviousPage { get; set; }

        public ApplicationPage PreviousPreviousPage { get; set; }

        public string BearerToken
        {
            set
            {
                _bearerToken = value;
                ServerService = new ServerService(_bearerToken);
                SignalRService = new SignalRService(_bearerToken);
            }
        }

        public string LastMessageReceived
        {
            get => _lastMessageUpdated;
            set
            {
                _lastMessageUpdated = value;
                Messenger.Default.Send(nameof(LastMessageReceived));
            }
        }

        public ServerService ServerService { get; set; }

        public SignalRService SignalRService { get; set; }

        public bool IsMenuDisplayed { get; set; }

        public bool AnyPopupVisible => IsMenuDisplayed;

        public bool IsUsernameLoaded => !string.IsNullOrEmpty(Username);

        public bool IsGameCreationSelected { get; set; }

        public WaitingRoom CurrentWaitingRoom { get; set; }

        public string Username { get; set; }

        public string UserId { get; set; }

        public int WindowMinimumWidth { get; set; } = 800;

        public int WindowMinimumHeight { get; set; } = 450;

        public Thickness InnerContentPadding => new Thickness(ResizeBorder);

        public int ResizeBorder { get; set; } = 2;

        public Thickness ResizeBorderThickness => new Thickness(ResizeBorder + OuterMarginSize);

        public int OuterMarginSize
        {
            get => Window.WindowState == WindowState.Maximized ? 0 : _outerMarginSize;
            set => _outerMarginSize = value;
        }

        public Thickness OuterMarginSizeThickness => new Thickness(OuterMarginSize);

        public int WindowRadius
        {
            get => Window.WindowState == WindowState.Maximized ? 0 : _windowRadius;
            set => _windowRadius = value;
        }

        public CornerRadius WindowCornerRadius => new CornerRadius(WindowRadius);

        public int TitleHeight { get; set; } = 42;

        public GridLength TitleHeightGridLength => new GridLength(TitleHeight + ResizeBorder);

        #endregion

        #region Commands

        public ICommand MinimizeCommand { get; set; }

        public ICommand MaximizeCommand { get; set; }

        public ICommand CloseCommand { get; set; }

        public ICommand MenuCommand { get; set; }

        public ICommand DisplayMenuCommand { get; set; }

        public ICommand PopupClickawayCommand { get; set; }

        public ICommand LogoutCommand { get; set; }

        public ICommand DisplayProfileCommand { get; set; }

        #endregion

        #region Contructor

        public MainWindowViewModel(MainWindow window)
        {
            Window = window;

            Window.StateChanged += (sender, e) =>
            {
                OnPropertyChanged(nameof(ResizeBorderThickness));
                OnPropertyChanged(nameof(OuterMarginSize));
                OnPropertyChanged(nameof(OuterMarginSizeThickness));
                OnPropertyChanged(nameof(WindowRadius));
                OnPropertyChanged(nameof(WindowCornerRadius));
            };

            MinimizeCommand = new RelayCommand<object>((o) => Window.WindowState = WindowState.Minimized, (o) => true);
            MaximizeCommand = new RelayCommand<object>((o) => Window.WindowState ^= WindowState.Maximized, (o) => true);
            CloseCommand = new RelayCommand<object>((o) => Window.Close(), (o) => true);
            DisplayMenuCommand = new RelayCommand<object>((o) => DisplayMenu(), (o) => true);
            PopupClickawayCommand = new RelayCommand<object>((o) => PopupClickAway(), (o) => true);
            LogoutCommand = new RelayCommand<object>((o) => Logout(), (o) => true);
            DisplayProfileCommand = new RelayCommand<object>((o) => ChangePage(ApplicationPage.ProfilePage), (o) => true);
            // Not working properly for the moment and not completely necessary
            // MenuCommand = new RelayCommand<object>((o) => SystemCommands.ShowSystemMenu(Window, new Point(Mouse.GetPosition(Window).X + Window.Left, Mouse.GetPosition(Window).Y + Window.Top)), (o) => true);

            var resizer = new WindowResizer(Window);
        }

        #endregion

        #region Functions

        private async void Logout()
        {
            ChangePage(ApplicationPage.LoadingPage);
            BrowserResultType browserResult = await Client.LogoutAsync();

            if (browserResult == BrowserResultType.Success)
            {
                Username = string.Empty;
                IsMenuDisplayed = false;
                ChangePage(ApplicationPage.WelcomePage);
                Window.Show();
            }
            else
            {
                MessageBox.Show(Resources.LogoutErrorMessage);
            }
        }

        private void DisplayMenu()
        {
            if (Window.DropDown.Content.Children.Count == 0)
            {
                Button logoutButton = new Button
                {
                    Style = (Style) Application.Current.Resources["DropdownElementStyle"],
                    Content = Resources.Logout,
                    Command = LogoutCommand,
                };

                Window.DropDown.Content.Children.Add(logoutButton);
            }
            IsMenuDisplayed ^= true;
        }

        public void PopupClickAway()
        {
            IsMenuDisplayed = false;
        }

        public void ChangePage(ApplicationPage page)
        {
            if (CurrentPage == page) return;

            if (_pagesNotInNavigation.Contains(page))
            {
                PreviousPreviousPage = PreviousPage;
                PreviousPage = CurrentPage;
            }
            else if (_pagesNotInNavigation.Contains(CurrentPage) && CurrentPage != ApplicationPage.LoadingPage)
            {
                PreviousPage = PreviousPreviousPage;
            }
            else if (CurrentPage != ApplicationPage.LoadingPage)
            {
                PreviousPage = CurrentPage;
            }

            CurrentPage = page;
        }

        #endregion
    }
}