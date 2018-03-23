using System;

namespace GameLogic
{
    /// <summary>
    /// Базовое представление любой карты
    /// </summary>
    public abstract class Card : IComparable<Card>
    {
        /// <summary>
        /// Редкость карты
        /// </summary>
        public readonly Rarity CardRarity;

        /// <summary>
        /// Уникальный идентификатор карты в системе
        /// </summary>
        public readonly int id = -1;
        
        private int cost = -1;
        /// <summary>
        /// Стоимость карты для вызова
        /// </summary>
        public int Cost
        {
            get { return cost; }
            private set { if (value < 0) throw new ArgumentException("Invalid cost"); cost = value; }
        }
        
        public Card(int id, int cost, Rarity rarity)
        {
            if (id < 0) throw new ArgumentException("Invalid ID");
            this.id = id;

            try
            {   
                CardRarity = rarity;
                Cost = cost;
            }
            catch (Exception e)
            { throw e; }
        }

        //реализация IComparable
        public virtual int CompareTo(Card other)
        {
            if (other is Card)
            {
                var comp = other as Card;

                if (CardRarity == comp.CardRarity)
                {
                    if (Cost > comp.Cost) return 1;
                    else if (Cost < comp.Cost) return -1;
                    return 0;
                }

                else if (CardRarity > comp.CardRarity) return 1;
                return -1;
            }

            throw new FormatException();
        }
    }
}