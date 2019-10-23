using PolyPaint.ViewModels;

namespace PolyPaint.Views.Pages
{
    /// <summary>
    /// Interaction logic for WelcomePage.xaml
    /// </summary>
    public partial class WelcomePage : BasePage
    {
        public WelcomePage()
        {
            InitializeComponent();

            DataContext = new WelcomePageViewModel(this);
        }
    }
}
