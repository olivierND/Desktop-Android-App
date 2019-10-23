using System.Windows;
using PolyPaint.Converters;
using PolyPaint.Models;
﻿using System.Collections.Generic;
using PolyPaint.ViewModels;
using PolyPaint.Views.Controls;

namespace PolyPaint.Views.Pages
{
    /// <summary>
    /// Interaction logic for LobbyPage.xaml
    /// </summary>
    public partial class LobbyPage : BasePage
    {
        private string[] _joueurs = { "joueur 1", "joueur 2" };
        private Game[] _games = { new Game("game 1", "classique", "2"), new Game("game 1", "classique", "2"), new Game("game 1", "classique", "2"), new Game("game 1", "classique", "2"), new Game("game 1", "classique", "2"), new Game("game 1", "classique", "2"), new Game("game 1", "classique", "2"), new Game("game 1", "classique", "2"), new Game("game 1", "classique", "2"), new Game("game 1", "classique", "2"), new Game("game 1", "classique", "2"), new Game("game 1", "classique", "2"), new Game("game 1", "classique", "2"), new Game("game 1", "classique", "2"), new Game("game 1", "classique", "2"), new Game("game 1", "classique", "2"),  };

        public LobbyPage()
        {
            InitializeComponent();
            DataContext = new LobbyPageViewModel(this);
        }

        public void DisplayPlayers()
        {
            foreach (string j in _joueurs)
            {
                PlayerGridElement grid = new PlayerGridElement {PlayerName = {Text = j}};
                Players.Children.Add(grid);
            }
        }

        public GameGridElement DisplayGame(WaitingRoom r)
        {
            GameModeToStringConverter converter = new GameModeToStringConverter();

            GameGridElement grid = new GameGridElement();

            ((GameGridElementViewModel) grid.DataContext).WaitingRoom = r;

            Games.Children.Add(grid);

            return grid;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (DataContext != null) await ((LobbyPageViewModel)DataContext)?.DisplayWaitingRooms();
        }
    }
}
