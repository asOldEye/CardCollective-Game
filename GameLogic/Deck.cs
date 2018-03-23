using System;
using System.Collections.Generic;

namespace GameLogic
{
    /// <summary>
    /// Колода карт
    /// </summary>
    public struct Deck<T> where T : Card
    {
        //карты в колоде
        readonly List<T> cards;
        
        /// <summary>
        /// Количество карт в колоде
        /// </summary>
        public int Count
        { get { return cards.Count; } }

        /// <summary>
        /// Перемешивание колоды
        /// </summary>
        public void Randomize()
        {
            if (cards.Count == 0)
                throw new NullReferenceException("Empty deck");

            Random random = new Random();

            for (int i = 0; i < cards.Count; i++)
            {
                var f = random.Next(0, cards.Count);

                var buffer = cards[f];
                cards[f] = cards[i];
                cards[i] = buffer;
            }
        }

        /// <summary>
        /// Сортировка колоды по параметру сравнения, либо по умолчанию
        /// </summary>
        /// <param name="comparition">Параметр сравнения</param>
        public void Sort(Comparison<T> comparition = null)
        {
            if (cards.Count == 0)
                throw new NullReferenceException("Empty deck");

            if (comparition == null)
                cards.Sort();
            else
                try
                {
                    cards.Sort(comparition);
                }
                catch (Exception e) { throw e; }
        }

        //конструктор для колоды карт
        public Deck(List<T> cards = null)
        {
            this.cards = new List<T>();

            if (cards == null) return;

            foreach (var card in cards)
                if (card == null) throw new NullReferenceException("No valid cards in deck");

            this.cards = new List<T>(cards);
        }

        /// <summary>
        /// Добавление карты в колоду
        /// </summary>
        /// <param name="card">Добавляемая карта</param>
        /// <param name="index">Индекс, по которому добавляется карта</param>
        public void AddCard(T card, int index = 0)
        {
            if (card == null) throw new NullReferenceException("Null card");

            if (index >= 0 && index < Count)
                cards.Insert(index, card);
            else
                throw new IndexOutOfRangeException("Wrong index");
        }

        /// <summary>
        /// Удаление карты из колоды
        /// </summary>
        /// <param name="card">Удаляемая карта</param>
        /// <returns>Удалена ли карта из колоды</returns>
        public bool RemoveCard(T card)
        {
            if (cards.Count == 0)
                throw new NullReferenceException("Empty deck");

            if (card == null) throw new NullReferenceException("Null card");

            return cards.Remove(card);
        }

        /// <summary>
        /// Вытащить верхнюю карту из колоды
        /// </summary>
        /// <returns>Возвращает вытащенную карту</returns>
        public Card PullCard()
        {
            if (cards.Count == 0)
                throw new NullReferenceException("Empty or null deck");

            var card = cards[0];
            cards.RemoveAt(0);
            return card;
        }

        /// <summary>
        /// Представляет колоду карт массивом
        /// </summary>
        /// <returns>Массив карт в колоде</returns>
        public T[] ToArray()
        {
            return cards.ToArray();
        }
    }
}