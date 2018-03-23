namespace GameLogic
{
    /// <summary>
    /// Позволяет наносить урон и лечить
    /// </summary>
    public interface IDestroyable
    {
        /// <summary>
        /// Максимальное здоровье уничтожаемого объекта
        /// </summary>
        int HealthMax
        {
            get;
        }

        /// <summary>
        /// Здоровье уничтожаемого объекта
        /// </summary>
        int Health
        {
            get;
        }
        
        /// <summary>
        /// Изменение здоровья объекта
        /// </summary>
        /// <param name="health">Количество изменяемых единиц здоровья</param>
        void DeltaHealth(int delta);

        /// <summary>
        /// Событие, вызывающееся при смерти
        /// </summary>
        event InGameEvent OnDeath;

        /// <summary>
        /// Событие, вызывающееся при изменении количества здоровья
        /// </summary>
        event InGameEvent OnHealthChanged;
    }
}