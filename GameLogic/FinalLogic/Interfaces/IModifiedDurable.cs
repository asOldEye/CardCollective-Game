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
    }
}