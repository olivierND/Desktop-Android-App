using PolyPaint.ViewModels;

namespace PolyPaint.Views.Pages
{
    /// <summary>
    /// Interaction logic for InviteFriendOrBotPage.xaml
    /// </summary>
    public partial class InviteFriendOrBotPage : BasePage
    {
        public InviteFriendOrBotPage()
        {
            InitializeComponent();

            DataContext = new InviteFriendOrBotPageViewModel();
        }
    }
}
