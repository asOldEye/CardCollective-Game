using System.Collections.Generic;

namespace GameLogic
{
    /// <summary>
    /// Объекты, на которые можно наложить долговременные модификаторы
    /// </summary>
    /// <typeparam name="T">Тип модификаторов</typeparam>
    public interface IModified
    {
        /// <summary>
        /// Список модификаторов
        /// </summary>
        List<Modifier> Modifiers { get; }

        /// <summary>
        /// Добавление нового модификатора
        /// </summary>
        /// <param name="modifier">Модификатор</param>
        /// <param name="time">Время действия</param>
        void TakeModifier(Modifier modifier);

        /// <summary>
        /// Выполняет модификаторы, либо убирает их по истечении ходов
        /// </summary>
        void TurnRun();
    }
}
