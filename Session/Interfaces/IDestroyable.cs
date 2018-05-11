using AuxiliaryLibrary;

namespace CardCollectiveSession
{
    /// <summary>
    /// Позволяет наносить урон и лечить
    /// </summary>
    public interface IDestroyable
    {
        /// <summary>
        /// Максимальное здоровье объекта
        /// </summary>
        int HealthMax
        { get; }
        /// <summary>
        /// Здоровье уничтожаемого объекта
        /// </summary>
        int Health
        { get; }
        
        /// <summary>
        /// Изменение здоровья объекта
        /// </summary>
        /// <param name="health">Количество изменяемых единиц здоровья</param>
        void DeltaHealth(int delta);

        /// <summary>
        /// Событие, вызывающееся при смерти
        /// </summary>
        event NonParametrizedEventHandler<IDestroyable> OnDeath;
        /// <summary>
        /// Событие, вызывающееся при изменении количества здоровья
        /// </summary>
        event ParametrizedEventHandler<IDestroyable, int> OnHealthChanged;
    }
}