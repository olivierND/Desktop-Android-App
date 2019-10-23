using System.Windows.Controls;
using PolyPaint.ViewModels;

namespace PolyPaint.Views.Controls
{
    /// <summary>
    /// Interaction logic for GameGridElement.xaml
    /// </summary>
    public partial class GameGridElement : UserControl
    {
        public GameGridElement()
        {
            InitializeComponent();

            DataContext = new GameGridElementViewModel(this);
        }
    }
}
