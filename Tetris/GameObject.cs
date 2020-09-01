using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public enum EGameObjectType
    {
        Block,
        Stick,
        L_form,
        Triangle,
        Z_form
    }

    public struct ObjectPositions
    {
        public int X;
        public int Y;
    }

    public class GameObject
    {
        public const int size = 4;

        public GameObject(EGameObjectType type)
        {
            this.ObjectType = type;
        }
        public bool isStop = true;
        public EGameObjectType ObjectType;
        public ObjectPositions[] Positions;
    }
}
