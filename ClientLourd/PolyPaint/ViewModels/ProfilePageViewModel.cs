using System.Windows.Input;
using PolyPaint.Utils;
using PolyPaint.Views.Pages;

namespace PolyPaint.ViewModels
{
    public class ProfilePageViewModel : BaseViewModel
    {
        private ProfilePage _page;

        public string Username => MainWindowViewModel.Username;

        public ProfilePageViewModel(ProfilePage page)
        {
            _page = page;
        }
    }
}