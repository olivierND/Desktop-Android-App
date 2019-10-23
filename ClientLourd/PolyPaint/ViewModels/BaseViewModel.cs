using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using PolyPaint.DataModels;
using PolyPaint.Utils;
using PolyPaint.Views.Controls;
using Application = System.Windows.Application;

namespace PolyPaint.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        #region Private Properties
        private readonly List<ApplicationPage> _warningBeforePreviousPages = new List<ApplicationPage>
        {
            ApplicationPage.WaitingRoomPage,
        }; 
        #endregion

        #region Public Properties

        public PreviousPageNavigationWarningPopup PreviousPageNavigationWarningPopup { get; set; }

        public MainWindowViewModel MainWindowViewModel = (MainWindowViewModel)Application.Current.MainWindow?.DataContext;

        public event PropertyChangedEventHandler PropertyChanged = (sender, e) => { }; 

        #endregion

        #region Commands
        public ICommand ReturnToPreviousPageCommand { get; set; }

        public ICommand DisplayPopupIfNeededCommand { get; set; }

        public ICommand HideWarningPopupCommand { get; set; }
        #endregion

        #region Constructor
        public BaseViewModel()
        {
            ReturnToPreviousPageCommand = new RelayCommand<object>((o) => ReturnToPreviousPage(), (o) => true);
            DisplayPopupIfNeededCommand = new RelayCommand<object>((o) => DisplayPopupIfNeeded(), (o) => true);
            HideWarningPopupCommand = new RelayCommand<object>((o) => HideWarningPopup(), (o) => true);
        }
        #endregion

        #region Functions
        private void HideWarningPopup()
        {
            PreviousPageNavigationWarningPopup.Popup.IsOpen = false;
            PreviousPageNavigationWarningPopup = null;
        }

        private void DisplayPopupIfNeeded()
        {
            if (PreviousPageNavigationWarningPopup != null && PreviousPageNavigationWarningPopup.Popup.IsOpen) return;

            if (_warningBeforePreviousPages.Contains(MainWindowViewModel.CurrentPage))
            {
                PreviousPageNavigationWarningPopup = new PreviousPageNavigationWarningPopup
                {
                    Popup = { IsOpen = true }
                };
                return;
            }

            ReturnToPreviousPage();
        }

        private void ReturnToPreviousPage()
        {
            if (PreviousPageNavigationWarningPopup != null)
            {
                PreviousPageNavigationWarningPopup.Popup.IsOpen = false;
                PreviousPageNavigationWarningPopup = null;
            }

            MainWindowViewModel.ChangePage(MainWindowViewModel.PreviousPage);
        }

        public void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        } 
        #endregion
    }
}