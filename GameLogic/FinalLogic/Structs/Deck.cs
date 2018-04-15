using System;
using System.Collections;
using System.Collections.Generic;

namespace GameLogic
{
    /// <summary>
    /// Колода карт
    /// </summary>
    public struct Deck<T> : IEnumerable<T> where T : Card
    {
        List<T> cards;

        public Deck(IEnumerable<T> cards = null)
        {
            if (cards == null)
                this.cards = new List<T>();
            else
            {
                foreach (var card in cards)
                    if (card == null) throw new NullReferenceException("Not valid card in deck");

                this.cards = new List<T>(cards);
            }
        }

        /// <summary>
        /// Количество карт в колоде
        /// </summary>
        public int Count
        { get { return cards.Count; } }

        /// <summary>
        /// Сортировка колоды по параметру сравнения, либо по умолчанию
        /// </summary>
        /// <param name="comparition">Параметр сравнения</param>
        public void Sort(Comparison<T> comparison = null)
        {
            if (comparison == null) cards.Sort();
            else
                try
                {
                    cards.Sort(comparison);
                }
                catch { throw; }
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
            else throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Удаление карты из колоды
        /// </summary>
        /// <param name="card">Удаляемая карта</param>
        /// <returns>Удалена ли карта из колоды</returns>
        public bool RemoveCard(T card)
        { return cards.Remove(card); }

        /// <summary>
        /// Представляет колоду карт массивом
        /// </summary>
        /// <returns>Массив карт в колоде</returns>
        public T[] ToArray()
        { return cards.ToArray(); }

        public IEnumerator<T> GetEnumerator()
        { return ((IEnumerable<T>)cards).GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator()
        { return ((IEnumerable<T>)cards).GetEnumerator(); }
    }
}