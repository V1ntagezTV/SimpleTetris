using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace Tetris
{
    public class Game
    {
        public static Random rnd = new Random();

        public const int Height = 20;
        public const int Width = 10;
        public List<List<int>> GameFields = new List<List<int>>(20);
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

        public void AddGameObject()
        {
            CheckFilledLines();
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
                    CurrentDownObject.Positions[2] = new ObjectPositions() { X = 2, Y = 4 };
                    CurrentDownObject.Positions[3] = new ObjectPositions() { X = 1, Y = 5 };
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
            var updatedList = new List<List<int>>(20);
            var removedLines = 0;
            for (int x = 0; x < Game.Height; x++)
            {
                if (GameFields[x].All(num => num == 1))
                {
                    GameFields.Remove(GameFields[x]);
                    removedLines++;
                    GameFields.Insert(0, new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
                }
            }
            return removedLines;
        }
    }
}
