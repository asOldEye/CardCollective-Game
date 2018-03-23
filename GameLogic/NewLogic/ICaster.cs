namespace GameLogic
{
    /// <summary>
    /// Представляет создание заклинаний
    /// </summary>
    interface ICaster
    {
        /// <summary>
        /// Количество маны
        /// </summary>
        int Mana
        { get; }

        /// <summary>
        /// Изменение здоровья объекта
        /// </summary>
        /// <param name="health">Количество изменяемых единиц здоровья</param>
        void DeltaMana(int delta);

        /// <summary>
        /// Создать заклинание
        /// </summary>
        /// <param name="spell">Заклинание из списка доступных</param>
        /// <param name="target">Цель атаки</param>
        void Cast(SpellCard spell, IDestroyable target = null);

        /// <summary>
        /// Список доступных заклинаний
        /// </summary>
        Deck<SpellCard> Spells
        { get; }

        /// <summary>
        /// Событие, вызывающееся при изменении силы атаки
        /// </summary>
        event GameEvent ManaChanged;
    }
}
