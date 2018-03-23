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
        /// Добавление нового модификатора
        /// </summary>
        /// <param name="modifier">Модификатор</param>
        /// <param name="time">Время действия</param>
        void TakeModifier(Modifier modifier);
    }
}