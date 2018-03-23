﻿namespace GameLogic
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
        /// Получение урона объектом
        /// </summary>
        /// <param name="damage">Количество получаемых единиц урона</param>
        void TakeDamage(int damage);

        /// <summary>
        /// Изменение здоровья объекта
        /// </summary>
        /// <param name="health">Количество изменяемых единиц здоровья</param>
        void DeltaHealth(int delta);

        /// <summary>
        /// Событие, вызывающееся при смерти
        /// </summary>
        event GameEvent Death;

        /// <summary>
        /// Событие, вызывающееся при изменении количества здоровья
        /// </summary>
        event GameEvent HealthChanged;
    }
}