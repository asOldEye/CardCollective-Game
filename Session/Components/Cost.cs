using System;

namespace CardCollectiveSession
{
    /// <summary>
    /// Стоимость карты
    /// </summary>
    public class Cost : Component
    {
        /// <summary>
        /// Стоимость карты
        /// </summary>
        public int Value { get; }
        public Cost(int cost)
        {
            if ((Value = cost) < 0)
                throw new ArgumentException("Cost must be moe than zero");
        }
    }
}