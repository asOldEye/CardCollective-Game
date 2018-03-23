using System.Collections.Generic;

namespace GameLogic
{
    /// <summary>
    /// Объекты, на которые можно наложить долговременные модификаторы
    /// </summary>
    /// <typeparam name="T">Тип модификаторов</typeparam>
    interface IModifiedObject<T> where T : Modifier
    {
        /// <summary>
        /// Очередь модификаторов и соотв. им оставшегося времени действия
        /// </summary>
        Queue<KeyValuePair<T, int>> Modifiers { get; }

        /// <summary>
        /// Добавление нового модификатора
        /// </summary>
        /// <param name="modifier">Модификатор</param>
        /// <param name="time">Время действия</param>
        void AddModifier(Modifier modifier, int time);

        /// <summary>
        /// Выполняет модификаторы, либо убирает их по истечении ходов
        /// </summary>
        void TurnTick();
    }
    
}
