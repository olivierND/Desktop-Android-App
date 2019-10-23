using System;
using System.Windows.Controls;
using PolyPaint.ViewModels;

namespace PolyPaint.Views.Controls
{
    /// <summary>
    /// Interaction logic for GameCreationDialog.xaml
    /// </summary>
    public partial class GameCreationDialog : UserControl
    {
        public GameCreationDialog()
        {
            InitializeComponent();

            DataContext = new GameCreationDialogViewModel(this);
        }

        private void GameName_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ((GameCreationDialogViewModel) DataContext).IsGameNameFilled =
                !string.IsNullOrEmpty(GameName.Text);
        }
    }
}
