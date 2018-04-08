using System;

namespace GameLogic
{
    /// <summary>
    /// Позиция объекта на игровом поле
    /// </summary>
    public class Position
    {
        int x;
        /// <summary>
        /// Координата Х объекта
        /// </summary>
        public int X
        {
            get { return x; }
            set
            {
                if (value < 0) throw new ArgumentException();
                x = value;
            }
        }
        
        int y;
        /// <summary>
        /// Координата Y объекта
        /// </summary>
        public int Y
        {
            get { return y; }
            set
            {
                if (value < 0) throw new ArgumentException();
                y = value;
            }
        }

        public Position(int x, int y)
        {
            if (x < 0 || y < 0) throw new ArgumentException();
            this.x = x;
            this.y = y;
        }

        public static int Distance(Position p1, Position p2)
        {
            return (int)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }
    }
}