namespace GameLogic
{
    /// <summary>
    /// Объекты, на которые можно наложить долговременные модификаторы
    /// </summary>
    public interface IModified
    {
        /// <summary>
        /// Добавление нового модификатора
        /// </summary>
        /// <param name="modifier">Модификатор</param>
        void TakeModifier(Modifier modifier);
    }
}