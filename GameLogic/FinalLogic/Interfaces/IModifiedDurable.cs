using System.Collections.Generic;

namespace GameLogic
{
    public interface IModifiedDurable : IModified
    {
        /// <summary>
        /// Список модификаторов
        /// </summary>
        List<DurableModifier> Modifiers { get; }

        /// <summary>
        /// Выполняет модификаторы, либо убирает их по истечении ходов
        /// </summary>
        void TurnRun();

        /// <summary>
        /// Удаляет модификатор
        /// </summary>
        /// <param name="modifier"></param>
        void DelModifier(DurableModifier modifier);

        /// <summary>
        /// Вызывается при добавлении модификатора
        /// </summary>
        event InGameEvent OnModifierAdd;
        /// <summary>
        /// Вызывается при удалении модификатора
        /// </summary>
        event InGameEvent OnModifierRemove;
    }
}