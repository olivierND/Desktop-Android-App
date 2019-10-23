using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using PolyPaint.DataModels;
using PolyPaint.Models;
using PolyPaint.Utils;
using PolyPaint.Views.Controls;

namespace PolyPaint.ViewModels
{
    public class GameCreationDialogViewModel: BaseViewModel
    {
        #region Private Properties

        private readonly GameCreationDialog _control;

        #endregion

        #region Public Properties
        public bool IsGameNameFilled { get; set; }

        public bool IsAllRequiredSectionsFilled => IsGameNameFilled;
        #endregion

        #region Commands
        public ICommand CancelGameCreationCommand { get; set; }

        public ICommand CreateWaitingRoomCommand { get; set; }
        #endregion

        #region Constructor
        public GameCreationDialogViewModel(GameCreationDialog control)
        {
            _control = control;
            CancelGameCreationCommand = new RelayCommand<object>((o) => MainWindowViewModel.IsGameCreationSelected = false, (o) => true);
            CreateWaitingRoomCommand = new RelayCommand<object>(async (o) => await CreateWaitingRoom(), (o) => true);
        }

        #endregion

        #region Functions

        public async Task CreateWaitingRoom()
        {
            MainWindowViewModel.IsGameCreationSelected = false;

            MainWindowViewModel.ChangePage(ApplicationPage.LoadingPage);

            HttpContent resContent = await MainWindowViewModel.ServerService.CreateNewChannel(_control.GameName.Text, true);

            if (resContent == null)
            {
                return;
            }

            Channel c = await resContent.ReadAsAsync(typeof(Channel)) as Channel;

            MainWindowViewModel.CurrentWaitingRoom = new WaitingRoom
            {
                Channel = c,
                Mode = GameMode.Classic
            };

            await AddCurrentUserAsHost(c);

            MainWindowViewModel.ChangePage(ApplicationPage.WaitingRoomPage);
        }

        private async Task AddCurrentUserAsHost(Channel c)
        {
            await MainWindowViewModel.SignalRService.JoinChannel(MainWindowViewModel.Username, c.id);

            MainWindowViewModel.CurrentWaitingRoom.Host = new Player
            {
                Name = MainWindowViewModel.Username,
                UserId = MainWindowViewModel.UserId
            };

            MainWindowViewModel.CurrentWaitingRoom.NbPlayer++;
        }

        #endregion
    }
}