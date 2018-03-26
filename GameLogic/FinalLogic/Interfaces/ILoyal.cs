namespace GameLogic
{
    /// <summary>
    /// Лояльность карты
    /// </summary>
    interface ILoyal
    {
        /// <summary>
        /// Параметр лояльности карты
        /// </summary>
        int Loyality
        { get; }

        /// <summary>
        /// Изменение параметра лояльности
        /// </summary>
        /// <param name="delta"></param>
        void DeltaLoyality(int delta);

        /// <summary>
        /// Событие, вызывающееся при изменении количества здоровья
        /// </summary>
        event InGameEvent OnLoyalityChanged;
    }
}