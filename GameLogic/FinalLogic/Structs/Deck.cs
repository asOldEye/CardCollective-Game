using System;
using System.Collections;
using System.Collections.Generic;

namespace GameLogic
{
    /// <summary>
    /// Колода карт
    /// </summary>
    public struct Deck<T> : IEnumerable where T : Card
    {
        //карты в колоде
        List<T> cards;

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
            if (comparition == null)
                cards.Sort();
            else
                try
                {
                    cards.Sort(comparition);
                }
                catch (Exception e)
                { throw e; }
        }

        //конструктор для колоды карт
        public Deck(IEnumerable<T> cards = null)
        {
            this.cards = new List<T>();

            if (cards == null) return;

            foreach (var card in cards)
                if (card == null) throw new NullReferenceException("Not valid card in deck");

            this.cards = new List<T>(cards);
        }

        /// <summary>
        /// Добавление карты в колоду
        /// </summary>
        /// <param name="card">Добавляемая карта</param>
        /// <param name="index">Индекс, по которому добавляется карта</param>
        public void AddCard(T card, int index = 0)
        {
            if (card == null) throw new ArgumentNullException();

            if (index >= 0 && index <= Count)
                cards.Insert(index, card);
            else
                throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Удаление карты из колоды
        /// </summary>
        /// <param name="card">Удаляемая карта</param>
        /// <returns>Удалена ли карта из колоды</returns>
        public bool RemoveCard(T card)
        {
            return cards.Remove(card);
        }

        /// <summary>
        /// Вытащить верхнюю карту из колоды
        /// </summary>
        /// <returns>Возвращает вытащенную карту</returns>
        public T PullCard()
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

        //Реализация IEnumerable 
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable)cards).GetEnumerator();
        }
    }
}