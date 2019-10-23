using System.Windows;
using PolyPaint.Utils;
using PolyPaint.Models;
using PolyPaint.Views.Controls;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight.Messaging;
using PolyPaint.Converters;

namespace PolyPaint.ViewModels
{
    class GameGridElementViewModel : BaseViewModel
    {
        #region Private Properties
        private readonly GameGridElement _control;

        private readonly GameModeToStringConverter _gameModeToStringConverter;
        #endregion

        #region Public Properties
        public WaitingRoom WaitingRoom { get; set; }

        public string Name => WaitingRoom.Name;

        public string Mode => _gameModeToStringConverter.Convert(WaitingRoom.Mode, null, null, null)?.ToString();

        public int NbPlayers => WaitingRoom.NbPlayer;

        public string HostName => WaitingRoom.Host?.Name;

        public string TeamOnePlayerTwoName => WaitingRoom.TeamOnePlayerTwo?.Name;

        public string TeamTwoPlayerOneName => WaitingRoom.TeamTwoPlayerOne?.Name;

        public string TeamTwoPlayerTwoName => WaitingRoom.TeamTwoPlayerTwo?.Name;

        public bool IsExpanded { get; set; }

        public bool IsSelected { get; set; }
        #endregion

        #region Commands
        public ICommand TeamMenuDropDownCommand { get; set; }
        public ICommand IsSelectedGameCommand { get; set; }
        #endregion

        #region Constructor
        public GameGridElementViewModel(GameGridElement control)
        {
            _control = control;
            _gameModeToStringConverter = new GameModeToStringConverter();

            TeamMenuDropDownCommand = new RelayCommand<object>((o) => TeamMenuDropDown(), (o) => true);
            IsSelectedGameCommand = new RelayCommand<object>((o) => IsSelectedGame(), (o) => true);
        }
        #endregion

        #region Functions
        public void TeamMenuDropDown()
        {
            _control.TeamRow.Height = !IsExpanded
                ? _control.TeamRow.Height = GridLength.Auto
                : _control.TeamRow.Height = new GridLength(0);

            IsExpanded = !IsExpanded;
        }

        public void IsSelectedGame()
        {
            Messenger.Default.Send(nameof(IsSelectedGame) + WaitingRoom.Channel.id);
            _control.GameGrid.Background = (SolidColorBrush)Application.Current.Resources["BackgroundGrayBrush"];
        }

        public void UnSelectGame()
        {
            _control.GameGrid.Background = (SolidColorBrush)Application.Current.Resources["BackgroundLightBrush"];
        } 
        #endregion
    }
}
