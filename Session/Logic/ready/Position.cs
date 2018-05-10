using System;

namespace Session
{
    /// <summary>
    /// Позиция объекта на игровом поле
    /// </summary>
    public struct Position : IComparable<Position>
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
            if (x < 0 || y < 0) throw new ArgumentException("Values less than zero");
            this.x = x;
            this.y = y;
        }

        public static int Distance(Position p1, Position p2)
        {
            return (int)Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        /// <summary>
        /// Пустая позиция объекта
        /// </summary>
        public static Position Empty
        {
            get
            {
                var f = new Position
                {
                    x = -1,
                    y = -1
                };
                return f;
            }
        }

        public int CompareTo(Position other)
        {
            if (other.x > x && other.y > y)
                return 1;
            if (other.x < x && other.y < y)
                return -1;
            return 0;
        }

        public static bool operator ==(Position p1, Position p2)
        {
            if (p1.CompareTo(p2) == 0) return true;
            return false;
        }
        public static bool operator !=(Position p1, Position p2)
        {
            if (p1.CompareTo(p2) == 0) return false;
            return true;
        }
    }
}