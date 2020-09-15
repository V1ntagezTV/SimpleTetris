namespace Tetris
{
    public enum EFigureType
    {
        Block,
        Stick,
        L_form,
        Triangle,
        Z_form
    }

    public class Figure
    {
        public const int size = 4;

        public Figure(EFigureType type)
        {
            this.Type = type;
            if (type == EFigureType.Stick)
            {
                Positions = new int[4][]
                {
                    new int[4],
                    new int[4],
                    new int[4],
                    new int[4],
                };
                return;
            }
            Positions = new int[3][] 
            { 
                new int[3], 
                new int[3], 
                new int[3]
            };
        }
        public bool isStop = true;
        public EFigureType Type;
        public int[][] Positions;
        public int CenterHeight = 1;
        public int CenterWidth = 4;
    }
}
