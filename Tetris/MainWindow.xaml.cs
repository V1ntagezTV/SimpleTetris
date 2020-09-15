using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tetris
{

    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Game NewGame;
        List<List<Button>> ViewButtons = new List<List<Button>>();
        public MainWindow()
        {
            NewGame = new Game();
            InitializeComponent();
            CreateView();
            Start();
        }
        private async Task Start()
        {
            _ = Task.Run(async () =>
            {
                while (!NewGame.isOver)
                {
                    NewGame.DownObject();
                    UpdateScore(NewGame.Points);
                    await UpdateView();
                    Thread.Sleep(500);
                }
            });
        }


        private void CreateView()
        {
            for (int x = 0; x < Game.HEIGHT; x++)
            {
                ViewButtons.Add(new List<Button>());
                for (int y = 0; y < Game.WIDTH; y++)
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
                        ViewButtons[x][y].Background = NewGame.Map[x][y] == 1 ? Brushes.Aquamarine : Brushes.Gray;
                    }
                }
            });
        }

        private async Task DrawFigure()
        {
            await Dispatcher.InvokeAsync(() =>
            {
                for (int x = 0, drawX = -1; x < NewGame.FallingFigure.Positions.Length; x++, drawX++)
                {
                    for (int y = 0, drawY = -1; y < NewGame.FallingFigure.Positions[x].Length; y++, drawY++)
                    {
                        if (NewGame.FallingFigure.Positions[x][y] == 1)
                        {
                            var figure = NewGame.FallingFigure;
                            ViewButtons[drawX + figure.CenterHeight][drawY + figure.CenterWidth].Background = Brushes.White;
                        }
                    }
                }
            });
        }

        private void Roll_Click(object sender, RoutedEventArgs e)
        {
            NewGame.RollCurrentFigure();
            UpdateView();
        }

        private void Left_Click(object sender, RoutedEventArgs e)
        {
            if (NewGame.PushLeft()) UpdateHistory("Move left.");
            UpdateView();
        }

        private void Right_Click(object sender, RoutedEventArgs e)
        {
            if (NewGame.PushRight()) UpdateHistory("Move right.");
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
