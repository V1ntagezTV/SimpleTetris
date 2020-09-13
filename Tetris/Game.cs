using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tetris
{
    public class Game
    {
        public static Random rnd = new Random();

        public const int Height = 20;
        public const int Width = 10;

        public bool isOver = false;
        public List<List<int>> Map = new List<List<int>>(20);
        public int Points = 0;
        public Figure FallingFigure;

        public Game()
        {
            for (int ind = 0; ind < Height; ind++)
            {
                Map.Add(new List<int>(10) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            }
            AddGameObject(); // First game object.
        }

        public void AddInFieldsGameFigure(Figure figure)
        {
            for (int x = 0, drawX = -1; x < figure.Positions.Length; x++, drawX++)
            {
                for (int y = 0, drawY = -1; y < figure.Positions[x].Length; y++, drawY++)
                {
                    if (figure.Positions[x][y] == 1)
                    {
                        Map[drawX + figure.CenterHeight][drawY + figure.CenterWidth] = 1;
                    }
                }
            }
        }

        public void DownObject()
        {
            //if (FallingFigure.Positions.Any(row => row.Any(num => GameFields[]) ))
            //{
            //    AddInFieldsGameFigure(FallingFigure);
            //    AddGameObject();
            //    FallingFigure.isStop = true;
            //    return;
            //}
            //if (FallingFigure.Positions.Any(pos => GameFields[pos.X + 1][pos.Y] == 1))
            //{
            //    AddInFieldsGameFigure(FallingFigure);
            //    AddGameObject();
            //    FallingFigure.isStop = true;
            //    return;
            //}
           // if (FallingFigure.CenterHeight + 1 == Height) { return; }
            for (int x = 0, drawX = -1; x < FallingFigure.Positions.Length; drawX++, x++)
            {
                for (int y = 0, drawY = -1; y < FallingFigure.Positions[x].Length; drawY++, y++)
                {
                    if (FallingFigure.Positions[x][y] == 1 && drawX + FallingFigure.CenterHeight == Height - 1 || // Упоролся в самый низ
                        FallingFigure.Positions[x][y] == 1 && Map[drawX + FallingFigure.CenterHeight + 1][drawY + FallingFigure.CenterWidth] == 1) // упоролся в фигуру
                    {
                        AddInFieldsGameFigure(FallingFigure);
                        AddGameObject();
                        return;
                    }
                }
            }
            FallingFigure.CenterHeight++;
        }

        public bool PushLeft()
        {
            for (int x = 0, drawX = -1; x < FallingFigure.Positions.Length; drawX++, x++)
            {
                for (int y = 0, drawY = -1; y < FallingFigure.Positions[x].Length; drawY++, y++)
                {
                    if (FallingFigure.Positions[x][y] == 1 &&
                        drawY + FallingFigure.CenterWidth == 0)
                    {
                        return false;
                    }
                }
            }
            FallingFigure.CenterWidth--;
            return true;
        }

        public bool PushRight()
        {
            for (int x = 0, drawX = -1; x < FallingFigure.Positions.Length; drawX++, x++)
            {
                for (int y = 0, drawY = -1; y < FallingFigure.Positions[x].Length; drawY++, y++)
                {
                    if (FallingFigure.Positions[x][y] == 1 &&
                        drawY + FallingFigure.CenterWidth + 1 == Width)
                    {
                        return false;
                    }
                }
            }
            FallingFigure.CenterWidth++;
            return true;
        }

        public void AddGameObject()
        {
            this.Points += 100 * CheckFilledLines();
            switch (0)
            {
                case 0:
                    FallingFigure = new Figure(EFigureType.Triangle);
                    FallingFigure.Positions[0][1] = 1;
                    FallingFigure.Positions[1][0] = 1;
                    FallingFigure.Positions[1][1] = 1;
                    FallingFigure.Positions[1][2] = 1;
                    break;

                default:
                    throw new Exception();
            }
        }

        /// <summary>
        /// Return removed lines count.
        /// </summary>
        public int CheckFilledLines()
        {
            int count = 0;
            var updatedList = new List<List<int>>(20);
            for (int x = 0; x < Game.Height; x++)
            {
                if (Map[x].All(num => num == 1))
                {
                    Map.Remove(Map[x]);
                    count += 1;
                    Map.Insert(0, new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                }
            }
            return count;
        }

        public void RollCurrentFigure()
        {
            var result = new int[3][] { new int[3], new int[3], new int[3] };
            if (this.FallingFigure.Type == EFigureType.Block) { return; }
            if (this.FallingFigure.Type == EFigureType.Stick) { return; }


            for (int row = 0; row < FallingFigure.Positions.Length; row++)
            {
                for (int column = 0; column < FallingFigure.Positions[row].Length; column++)
                {
                    result[column][3 - 1 - row] = FallingFigure.Positions[row][column];
                }
            }
            FallingFigure.Positions = result;
        }
    }
}
