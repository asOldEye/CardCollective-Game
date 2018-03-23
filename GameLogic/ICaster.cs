using System.Collections.Generic;

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
    }
}
