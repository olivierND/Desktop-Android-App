using PolyPaint.ViewModels;

namespace PolyPaint.Views.Pages
{
    /// <summary>
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : BasePage
    {
        public ProfilePage()
        {
            InitializeComponent();

            DataContext = new ProfilePageViewModel(this);
        }
    }
}
