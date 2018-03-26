using System;

namespace GameLogic
{
    /// <summary>
    /// Позиция объекта на игровом поле
    /// </summary>
    public struct Position
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

        /// <summary>
        /// Дистанция между двумя позициями
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static int Distance(Position first, Position second)
        {
            return (int)Math.Sqrt((first.x - second.x) * (first.x - second.x) + (first.y - second.y) * (first.y - second.y));
        }
    }
}
