using System.Configuration;
using System.Linq;
using System.Windows;
using Auth0.OidcClient;
using IdentityModel.OidcClient;

namespace ClientLourd.Views
{
    /// <summary>
    /// Interaction logic for AuthenticationView.xaml
    /// </summary>
    public partial class AuthenticationView
    {
        public static Auth0Client Client = new Auth0Client(new Auth0ClientOptions
        {
            Domain = ConfigurationManager.AppSettings["Auth0:Domain"],
            ClientId = ConfigurationManager.AppSettings["Auth0:ClientId"],
            ClientSecret = ConfigurationManager.AppSettings["Auth0:ClientSecret"]
        });

        public AuthenticationView()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoginResult res = await Client.LoginAsync();

            if (!res.IsError)
            {
                MessagingClientView msgClientView =
                    MessagingClientView.GetInstance(res.User.Identity.Name, res.AccessToken);
                msgClientView.Show();
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
    }
}
