using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using PolyPaint.Constants;
using PolyPaint.DataModels;
using PolyPaint.Models;
using PolyPaint.Utils;
using PolyPaint.Views.Pages;

namespace PolyPaint.ViewModels
{
    public class WaitingRoomPageViewModel : BaseViewModel
    {
        private WaitingRoomPage _page;

        #region Public Properties

        public WaitingRoom WaitingRoom => MainWindowViewModel.CurrentWaitingRoom;

        public string WaitingForPlayersMessage { get; set; } = "En attente des autres joueurs...";

        #endregion;

        #region Commands
        public ICommand DisplayHostProfileCommand { get; set; }
        public ICommand DisplayTeamOnePlayerOneProfileOrInvitePageCommand { get; set; }
        public ICommand DisplayTeamTwoPlayerOneProfileOrInvitePageCommand { get; set; }
        public ICommand DisplayTeamTwoPlayerTwoProfileOrInvitePageCommand { get; set; }
        #endregion

        #region Constructor

        public WaitingRoomPageViewModel(WaitingRoomPage page)
        {
            _page = page;

            DisplayHostProfileCommand = new RelayCommand<object>((o) => DisplayHostProfile(), (o) => true);
            DisplayTeamOnePlayerOneProfileOrInvitePageCommand = new RelayCommand<object>((o) => DisplayTeamOnePlayerOneProfileOrInvitePage(), (o) => true);
            DisplayTeamTwoPlayerOneProfileOrInvitePageCommand = new RelayCommand<object>((o) => DisplayTeamTwoPlayerOneProfileOrInvitePage(), (o) => true);
            DisplayTeamTwoPlayerTwoProfileOrInvitePageCommand = new RelayCommand<object>((o) => DisplayTeamTwoPlayerTwoProfileOrInvitePage(), (o) => true);

            Messenger.Default.Register(this, delegate(string msg)
            {
                if (msg.StartsWith(SignalRFunctions.ReceiveMessage))
                {
                    ((ChatViewModel)_page.Chat.DataContext).DisplayLastMessageReceived(msg.Replace(SignalRFunctions.ReceiveMessage, string.Empty));
                }
            });
        }
        #endregion

        #region Functions

        private void DisplayHostProfile()
        {
            MainWindowViewModel.ChangePage(WaitingRoom.Host == null
                ? ApplicationPage.InviteFriendOrBotPage
                : ApplicationPage.ProfilePage);
        }

        private void DisplayTeamOnePlayerOneProfileOrInvitePage()
        {
            MainWindowViewModel.ChangePage(WaitingRoom.TeamOnePlayerTwo == null
                ? ApplicationPage.InviteFriendOrBotPage
                : ApplicationPage.ProfilePage);
        }
        private void DisplayTeamTwoPlayerOneProfileOrInvitePage()
        {
            MainWindowViewModel.ChangePage(WaitingRoom.TeamTwoPlayerOne == null
                ? ApplicationPage.InviteFriendOrBotPage
                : ApplicationPage.ProfilePage);
        }

        private void DisplayTeamTwoPlayerTwoProfileOrInvitePage()
        {
            MainWindowViewModel.ChangePage(WaitingRoom.TeamTwoPlayerTwo == null
                ? ApplicationPage.InviteFriendOrBotPage
                : ApplicationPage.ProfilePage);
        }

        #endregion
    }
}