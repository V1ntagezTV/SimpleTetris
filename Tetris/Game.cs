using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
    public class Game
    {
        public static Random rnd = new Random();

        public const int HEIGHT = 20;
        public const int WIDTH = 10;

        public bool isOver = false;
        public List<List<int>> Map = new List<List<int>>(20);
        public int Points = 0;
        public Figure FallingFigure;

        public Game()
        {
            for (int ind = 0; ind < HEIGHT; ind++)
            {
                Map.Add(new List<int>(10) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            }
            AddGameObject();
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
            for (int x = 0, drawX = -1; x < FallingFigure.Positions.Length; drawX++, x++)
            {
                for (int y = 0, drawY = -1; y < FallingFigure.Positions[x].Length; drawY++, y++)
                {
                    if (FallingFigure.Positions[x][y] == 1 && drawX + FallingFigure.CenterHeight == HEIGHT - 1 ||
                        FallingFigure.Positions[x][y] == 1 && Map[drawX + FallingFigure.CenterHeight + 1][drawY + FallingFigure.CenterWidth] == 1)
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
                    if (FallingFigure.Positions[x][y] == 1 &&
                        Map[FallingFigure.CenterHeight + drawX][FallingFigure.CenterWidth - 1 + drawY] == 1) 
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
                        drawY + FallingFigure.CenterWidth + 1 == WIDTH)
                    {
                        return false;
                    }
                    if (FallingFigure.Positions[x][y] == 1 &&
                        Map[FallingFigure.CenterHeight + drawX][FallingFigure.CenterWidth + 1 + drawY] == 1)
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
            Points += 100 * CheckFilledLines();
            switch (rnd.Next(0, 5))
            {
                case 0:
                    FallingFigure = new Figure(EFigureType.Triangle);
                    FallingFigure.Positions[0][1] = 1;
                    FallingFigure.Positions[1][0] = 1;
                    FallingFigure.Positions[1][1] = 1;
                    FallingFigure.Positions[1][2] = 1;
                    break;

                case 1:
                    FallingFigure = new Figure(EFigureType.L_form);
                    FallingFigure.Positions[0][0] = 1;
                    FallingFigure.Positions[1][0] = 1;
                    FallingFigure.Positions[1][1] = 1;
                    FallingFigure.Positions[1][2] = 1;
                    break;

                case 2:
                    FallingFigure = new Figure(EFigureType.Block);
                    FallingFigure.Positions[0][1] = 1;
                    FallingFigure.Positions[0][2] = 1;
                    FallingFigure.Positions[1][1] = 1;
                    FallingFigure.Positions[1][2] = 1;
                    break;

                case 3:
                    FallingFigure = new Figure(EFigureType.Z_form);
                    FallingFigure.Positions[0][0] = 1;
                    FallingFigure.Positions[1][0] = 1;
                    FallingFigure.Positions[1][1] = 1;
                    FallingFigure.Positions[2][1] = 1;
                    break;

                case 4:
                    FallingFigure = new Figure(EFigureType.Stick);
                    FallingFigure.Positions[2][0] = 1;
                    FallingFigure.Positions[2][1] = 1;
                    FallingFigure.Positions[2][2] = 1;
                    FallingFigure.Positions[2][3] = 1;
                    break;

                default:
                    throw new Exception();
            }
        }

        public int CheckFilledLines()
        {
            int count = 0;
            var updatedList = new List<List<int>>(20);
            for (int x = 0; x < Game.HEIGHT; x++)
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
            if (FallingFigure.Type == EFigureType.Block) { return; }
            int[][] result;
            if (FallingFigure.Type == EFigureType.Stick)
            {
                result = new int[4][] { new int[4], new int[4], new int[4], new int[4] };
            } 
            else
            {
                result = new int[3][] { new int[3], new int[3], new int[3] };
            }
            if (FallingFigure.CenterWidth == 0)
            {
                FallingFigure.CenterWidth++;
            }
            if (FallingFigure.CenterWidth == 9)
            {
                FallingFigure.CenterWidth--;
            }
            for (int row = 0; row < FallingFigure.Positions.Length; row++)
            {
                for (int column = FallingFigure.Positions[row].Length - 1; column >= 0; column--)
                {
                    result[column][FallingFigure.Positions[row].Length - 1 - row] = FallingFigure.Positions[row][column];
                }
            }
            FallingFigure.Positions = result;
        }
    }
}
