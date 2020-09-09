using System;
using System.Collections.Generic;
using System.Linq;

namespace Tetris
{
    public class Game
    {
        public static Random rnd = new Random();

        public const int Height = 20;
        public const int Width = 10;

        public bool isOver = false;
        public List<List<int>> GameFields = new List<List<int>>(20);
        public int Points = 0;
        public GameObject CurrentDownObject;

        public Game()
        {
            for (int ind = 0; ind < Height; ind++)
            {
                GameFields.Add(new List<int>(10) { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            }
            AddGameObject(); // First game object.
        }

        public void AddInFieldsGameObject(GameObject gameObject)
        {
            for (int ind = 0; ind < gameObject.Positions.Length; ind++)
            {
                var pos = gameObject.Positions[ind];
                GameFields[pos.X][pos.Y] = 1;
            }
        }

        public void DownObject()
        {
            if (CurrentDownObject.Positions.Any(pos => pos.X == 19))
            {
                AddInFieldsGameObject(CurrentDownObject);
                AddGameObject();
                CurrentDownObject.isStop = true;
                return;
            }
            if (CurrentDownObject.Positions.Any(pos => GameFields[pos.X + 1][pos.Y] == 1))
            {
                AddInFieldsGameObject(CurrentDownObject);
                AddGameObject();
                CurrentDownObject.isStop = true;
                return;
            }
            for (int ind = 0; ind < GameObject.size; ind++)
            {
                CurrentDownObject.Positions[ind].X++;
            }
        }

        public bool PushLeft()
        {
            if (CurrentDownObject.Positions.Any(pos => pos.Y-- == 0 || GameFields[pos.X][pos.Y--] == 1)) return false;
            var positions = CurrentDownObject.Positions;
            for (int ind = 0; ind < positions.Length; ind++)
            {
                positions[ind].Y--;
            }
            return true;
        }

        public bool PushRight()
        {
            if (CurrentDownObject.Positions.Any(pos => pos.Y++ == Game.Width - 1 || GameFields[pos.X][pos.Y++] == 1)) return false;
            var positions = CurrentDownObject.Positions;
            for (int ind = 0; ind < positions.Length; ind++)
            {
                positions[ind].Y++;
            }
            return true;
        }

        public void AddGameObject()
        {
            this.Points += 100 * CheckFilledLines();
            switch (rnd.Next(0, 5))
            {
                case 0:
                    CurrentDownObject = new GameObject(EGameObjectType.Block);
                    CurrentDownObject.Positions = new ObjectPositions[4];
                    CurrentDownObject.Positions[0] = new ObjectPositions() { X = 0, Y = 4 };
                    CurrentDownObject.Positions[1] = new ObjectPositions() { X = 0, Y = 5 };
                    CurrentDownObject.Positions[2] = new ObjectPositions() { X = 1, Y = 4 };
                    CurrentDownObject.Positions[3] = new ObjectPositions() { X = 1, Y = 5 };
                    break;
                case 1:
                    CurrentDownObject = new GameObject(EGameObjectType.L_form);
                    CurrentDownObject.Positions = new ObjectPositions[4];
                    CurrentDownObject.Positions[0] = new ObjectPositions() { X = 0, Y = 4 };
                    CurrentDownObject.Positions[1] = new ObjectPositions() { X = 1, Y = 4 };
                    CurrentDownObject.Positions[2] = new ObjectPositions() { X = 2, Y = 4 };
                    CurrentDownObject.Positions[3] = new ObjectPositions() { X = 2, Y = 5 };
                    break;
                case 2:
                    CurrentDownObject = new GameObject(EGameObjectType.Stick);
                    CurrentDownObject.Positions = new ObjectPositions[4];
                    CurrentDownObject.Positions[0] = new ObjectPositions() { X = 0, Y = 4 };
                    CurrentDownObject.Positions[1] = new ObjectPositions() { X = 1, Y = 4 };
                    CurrentDownObject.Positions[2] = new ObjectPositions() { X = 2, Y = 4 };
                    CurrentDownObject.Positions[3] = new ObjectPositions() { X = 3, Y = 4 };
                    break;
                case 3:
                    CurrentDownObject = new GameObject(EGameObjectType.Triangle);
                    CurrentDownObject.Positions = new ObjectPositions[4];
                    CurrentDownObject.Positions[0] = new ObjectPositions() { X = 0, Y = 4 };
                    CurrentDownObject.Positions[1] = new ObjectPositions() { X = 1, Y = 4 };
                    CurrentDownObject.Positions[3] = new ObjectPositions() { X = 1, Y = 5 };
                    CurrentDownObject.Positions[2] = new ObjectPositions() { X = 2, Y = 4 };
                    break;
                case 4:
                    CurrentDownObject = new GameObject(EGameObjectType.Z_form);
                    CurrentDownObject.Positions = new ObjectPositions[4];
                    CurrentDownObject.Positions[0] = new ObjectPositions() { X = 0, Y = 4 };
                    CurrentDownObject.Positions[1] = new ObjectPositions() { X = 0, Y = 5 };
                    CurrentDownObject.Positions[2] = new ObjectPositions() { X = 1, Y = 5 };
                    CurrentDownObject.Positions[3] = new ObjectPositions() { X = 1, Y = 6 };
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
                if (GameFields[x].All(num => num == 1))
                {
                    GameFields.Remove(GameFields[x]);
                    count += 1;
                    GameFields.Insert(0, new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                }
            }
            return count;
        }

        public void RollCurrentGameObject()
        {
            for (int ind = 0; ind < CurrentDownObject.Positions.Length; ind++)
            {
                var swapValue = CurrentDownObject.Positions[ind].Y;
                CurrentDownObject.Positions[ind].Y = CurrentDownObject.Positions[ind].X;
                CurrentDownObject.Positions[ind].X = Width - swapValue;
            }
        }
    }
}
