using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Schema;

namespace Tetris
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Game game;
        List<List<Button>> ViewButtons = new List<List<Button>>();
        public MainWindow()
        {
            game = new Game();
            InitializeComponent();
            CreateView();
            Start();
        }
        private async Task Start()
        {
            _ = Task.Run(async () =>
            {
                while (!game.isOver)
                {
                    game.DownObject();
                    UpdateScore(game.Points);
                    await UpdateView();
                    Thread.Sleep(500);
                }
            });
        }


        private void CreateView()
        {
            for (int x = 0; x < Game.Height; x++)
            {
                ViewButtons.Add(new List<Button>());
                for (int y = 0; y < Game.Width; y++)
                {
                    var button = new Button() { Background = Brushes.IndianRed };
                    ViewButtons[x].Add(button);
                    GameGrid.Children.Add(button);
                    Grid.SetRow(button, x);
                    Grid.SetColumn(button, y);
                }
            }
        }
        private async Task UpdateView()
        {
            await DrawMap();
            await DrawFigure();
        }

        private async Task DrawMap()
        {
            await Dispatcher.InvokeAsync(() =>
            {
                for (int x = 0; x < ViewButtons.Count; x++)
                {
                    for (int y = 0; y < ViewButtons[x].Count; y++)
                    {
                        ViewButtons[x][y].Background = game.GameFields[x][y] == 1 ? Brushes.IndianRed : Brushes.Gray;
                    }
                }
            });
        }

        private async Task DrawFigure()
        {
            await Dispatcher.InvokeAsync(() =>
            {
                for (int x = 0, drawX = -1; x < game.FallingFigure.Positions.Length; x++, drawX++)
                {
                    for (int y = 0, drawY = -1; y < game.FallingFigure.Positions[x].Length; y++, drawY++)
                    {
                        if (game.FallingFigure.Positions[x][y] == 1)
                        {
                            var figure = game.FallingFigure;
                            ViewButtons[drawX + figure.CenterHeight][drawY + figure.CenterWidth].Background = Brushes.IndianRed;
                        }
                    }
                }
            });
        }

        private void Roll_Click(object sender, RoutedEventArgs e)
        {
            game.RollCurrentGameObject();
            UpdateView();
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            if (game.PushLeft()) UpdateHistory("Move left.");
            UpdateView();
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            if (game.PushRight()) UpdateHistory("Move right.");
            UpdateView();
        }

        private void UpdateHistory(string news)
        {
            LB_History.Items.Add(new TextBlock() { Text = news });
        }

        private void UpdateScore(int points)
        {
            TB_GameScore.Dispatcher.Invoke(() => TB_GameScore.Text = $"{ points }");
        }
    }
}
