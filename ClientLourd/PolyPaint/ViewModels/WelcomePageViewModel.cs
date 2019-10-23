using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using IdentityModel.OidcClient;
using PolyPaint.DataModels;
using PolyPaint.Models;
using PolyPaint.Utils;
using PolyPaint.Views.Pages;

namespace PolyPaint.ViewModels
{
    public class WelcomePageViewModel : BaseViewModel
    {
        private WelcomePage _page;



        public ICommand OnConnectedButtonCommand { get; set; }

        public WelcomePageViewModel(WelcomePage page)
        {
            _page = page;

            OnConnectedButtonCommand = new RelayCommand<object>((o) => DisplayAuthenticationView(), (o) => true);
        }

        private async void DisplayAuthenticationView()
        {
            MainWindowViewModel.ChangePage(ApplicationPage.LoadingPage);

            LoginResult res = await MainWindowViewModel.Client.LoginAsync();

            if (!res.IsError)
            {
                MainWindowViewModel.Username = GetUserName(res);
                MainWindowViewModel.UserId = GetUserId(res);

                MainWindowViewModel.BearerToken = res.AccessToken;

                MainWindowViewModel.ChangePage(ApplicationPage.LobbyPage);

                // Wait for page to change completely before displaying window
                await Task.Delay(20);

                Application.Current.MainWindow?.Show();
            }
            else
            {
                if (res.Error == "UserCancel")
                {
                    Application.Current.Shutdown();
                }
                else
                {
                    MessageBox.Show(res.Error);
                }
            }
        }

        private string GetUserId(LoginResult res)
        {
            var handler = new JwtSecurityTokenHandler();
            var tokenS = handler.ReadToken(res.IdentityToken) as JwtSecurityToken;

            return tokenS?.Subject;
        }

        private string GetUserName(LoginResult res)
        {
            var handler = new JwtSecurityTokenHandler();

            return handler.ReadToken(res.AccessToken) is JwtSecurityToken tokenS && !tokenS.Claims.First(c => c.Type == "sub").Value.Contains("google")
                ? ((JwtSecurityToken)handler.ReadToken(res.IdentityToken)).Claims.First(c => c.Type == "nickname").Value
                : res.User.Identity.Name;
        }
    }
}