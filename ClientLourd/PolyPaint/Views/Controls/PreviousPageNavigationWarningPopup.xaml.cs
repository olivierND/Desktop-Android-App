using System.Windows.Controls;
using PolyPaint.ViewModels;

namespace PolyPaint.Views.Controls
{
    /// <summary>
    /// Interaction logic for PreviousPageNavigationWarningPopup.xaml
    /// </summary>
    public partial class PreviousPageNavigationWarningPopup : UserControl
    {
        public PreviousPageNavigationWarningPopup()
        {
            InitializeComponent();

            DataContext = new BaseViewModel
            {
                PreviousPageNavigationWarningPopup = this
            };
        }
    }
}
