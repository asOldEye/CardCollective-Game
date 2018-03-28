using System;

namespace GameLogic
{
    /// <summary>
    /// Базовое представление любой карты
    /// </summary>
    public abstract class Card : SessionObject, IComparable<Card>
    {
        /// <summary>
        /// Уникальный идентификатор карты в системе
        /// </summary>
        public readonly int id = -1;

        private readonly int cost = -1;
        /// <summary>
        /// Стоимость карты для вызова
        /// </summary>
        public int Cost
        { get { return cost; } }

        public Card(Session session, int id, int cost) : base(session)
        {
            if (id < 0) throw new ArgumentException("Invalid ID");

            if (cost < 0) throw new ArgumentException("Invalid cost");

            this.cost = cost;
            this.id = id;
        }

        //реализация IComparable
        public virtual int CompareTo(Card other)
        {
            if (other == null) throw new ArgumentNullException();

            if (Cost > other.Cost) return 1;
            else if (Cost < other.Cost) return -1;
            return 0;
        }
    }
}