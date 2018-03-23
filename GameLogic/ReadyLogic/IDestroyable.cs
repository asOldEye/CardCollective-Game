namespace GameLogic
{
    /// <summary>
    /// Позволяет наносить урон и лечить
    /// </summary>
    public interface IDestroyable
    {
        /// <summary>
        /// Здоровье уничтожаемого объекта
        /// </summary>
        int Health
        {
            get;
            set;
        }
        
        /// <summary>
        /// Получение урона объектом
        /// </summary>
        /// <param name="damage">количество получаемых единиц урона</param>
        /// <returns>true, если объект уничтожен атакой</returns>
        bool TakeDamage(int damage);

        /// <summary>
        /// Восстановление здоровья объекта
        /// </summary>
        /// <param name="health">каоличество получаемых единиц здоровья</param>
        void Cure(int health);
    }
}