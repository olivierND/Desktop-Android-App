using System.Windows;
using PolyPaint.ViewModels;

namespace PolyPaint.Views.Pages
{
    /// <summary>
    /// Interaction logic for WaitingRoomPage.xaml
    /// </summary>
    public partial class WaitingRoomPage : BasePage
    {
        public WaitingRoomPage()
        {
            InitializeComponent();

            DataContext = new WaitingRoomPageViewModel(this);
        }
    }
}
