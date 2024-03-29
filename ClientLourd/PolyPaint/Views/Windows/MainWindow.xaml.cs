﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PolyPaint.ViewModels;
using PolyPaint.Views.Controls;

namespace PolyPaint.Views.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainWindowViewModel(this);
        }

        private void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            GameCreationGrid.Children.Add(new GameCreationDialog());
        }
    }
}
