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
            await Dispatcher.InvokeAsync(() =>
            {
                for (int indX = 0; indX < ViewButtons.Count; indX++)
                {
                    for (int indY = 0; indY < ViewButtons[indX].Count; indY++)
                    {
                        ViewButtons[indX][indY].Background = game.GameFields[indX][indY] == 1 ? Brushes.Gray : Brushes.IndianRed;
                        for (int posInd = 0; posInd < game.CurrentDownObject.Positions.Length; posInd++)
                        {
                            var pos = game.CurrentDownObject.Positions[posInd];
                            ViewButtons[pos.X][pos.Y].Background = Brushes.Gray;
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
