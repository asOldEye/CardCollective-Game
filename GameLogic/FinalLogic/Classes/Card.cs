using System;

namespace GameLogic
{
    /// <summary>
    /// Базовое представление любой карты
    /// </summary>
    public abstract class Card : SessionObject, IComparable<Card>
    {
        /// <summary>
        /// Стоимость вызова карты 
        /// </summary>
        public readonly int Cost = -1;
        
        public Card(Session session, int cost) : base(session)
        {
            if (cost < 0) throw new ArgumentException("Invalid cost");
            Cost = cost;
        }
        
        public virtual int CompareTo(Card other)
        {
            try
            {
                if (Cost > other.Cost) return 1;
                else if (Cost < other.Cost) return -1;
                return 0;
            }
            catch { throw; }
        }
    }
}