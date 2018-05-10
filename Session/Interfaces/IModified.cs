using AuxiliaryLibrary;
using System.Collections.Generic;

namespace Session
{
    public interface IModified
    {
        /// <summary>
        /// Список модификаторов
        /// </summary>
        List<DurableModifier> Modifiers { get; }

        /// <summary>
        /// Добавляет модификатор
        /// </summary>
        void AddModifier(DurableModifier modifier);
        /// <summary>
        /// Удаляет модификатор
        /// </summary>
        void DelModifier(DurableModifier modifier);

        /// <summary>
        /// Вызывается при добавлении модификатора
        /// </summary>
        event ParametrizedEventHandler<IModified, DurableModifier> OnModifierAdded;
        /// <summary>
        /// Вызывается при удалении модификатора
        /// </summary>
        event ParametrizedEventHandler<IModified, DurableModifier> OnModifierRemoved;
    }
}