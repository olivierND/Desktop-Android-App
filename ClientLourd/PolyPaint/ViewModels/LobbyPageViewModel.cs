using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using PolyPaint.Views.Pages;
using System.Windows.Input;
using PolyPaint.DataModels;
using GalaSoft.MvvmLight.Messaging;
using PolyPaint.Models;
using PolyPaint.Utils;
using PolyPaint.Views.Controls;

namespace PolyPaint.ViewModels
{
    class LobbyPageViewModel: BaseViewModel
    {
        #region Private Properties
        private readonly LobbyPage _page;

        private WaitingRoom _currentSelectedWaitingRoom; 
        #endregion

        #region Public Properties

        public bool IsGameSelected { get; set; }

        public bool IsLoading { get; set; }

        public List<GameGridElement> Lobbies = new List<GameGridElement>();

        #endregion

        #region Commands
        public ICommand JoinLobbyCommand { get; set; }

        public ICommand DisplayGameCreationDialogCommand { get; set; }

        #endregion

        #region Constructor
        public LobbyPageViewModel(LobbyPage page)
        {
            _page = page;
            _page.DisplayPlayers();
            
            DisplayGameCreationDialogCommand = new RelayCommand<object>((o) => DisplayGameCreationDialog(), (o) => true);
            JoinLobbyCommand = new RelayCommand<object>(async(o)=> await JoinLobby(), (o)=>true);

            Messenger.Default.Register(this, delegate (string msg)
            {
                if (!msg.StartsWith(nameof(GameGridElementViewModel.IsSelectedGame))) return;

                foreach (GameGridElement g in Lobbies.Where(g => msg.EndsWith(((GameGridElementViewModel)g.DataContext).WaitingRoom.Channel.id)))
                {
                    foreach (GameGridElement gg in Lobbies)
                    {
                        ((GameGridElementViewModel)gg.DataContext).UnSelectGame();
                    }

                    IsGameSelected = true;
                    _currentSelectedWaitingRoom = ((GameGridElementViewModel)g.DataContext).WaitingRoom;
                }
            });
        }
        #endregion

        #region Functions

        public async Task JoinLobby()
        {
            MainWindowViewModel.CurrentWaitingRoom = _currentSelectedWaitingRoom;
            await MainWindowViewModel.SignalRService.JoinChannel(MainWindowViewModel.Username, _currentSelectedWaitingRoom.Channel.id);

            MainWindowViewModel.ChangePage(ApplicationPage.WaitingRoomPage);
        }

        public void DisplayGameCreationDialog()
        {
            MainWindowViewModel.IsGameCreationSelected = true;
        }

        public async Task DisplayWaitingRooms()
        {
            IsLoading = true;
            List<Channel> channels = await GetWaitingRooms();
            IsLoading = false;

            foreach (var r in channels.Select(c => new WaitingRoom
            {
                Channel = c,
                Mode = GameMode.Classic
            }))
            {
                Lobbies.Add(_page.DisplayGame(r));
            }
        }

        private async Task<List<Channel>> GetWaitingRooms()
        {
            HttpContent resContent = await MainWindowViewModel.ServerService.GetLobbies();

            if (resContent == null)
            {
                return null;
            }

            return await resContent.ReadAsAsync(typeof(List<Channel>)) as List<Channel>;
        } 
        #endregion
    }
}
