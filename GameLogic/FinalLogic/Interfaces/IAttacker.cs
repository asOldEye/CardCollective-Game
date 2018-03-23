namespace GameLogic
{
    /// <summary>
    /// Способный атаковать
    /// </summary>
    public interface IAttacker
    {
        /// <summary>
        /// Максимальная сила атаки
        /// </summary>
        int PowerMax
        { get; }

        /// <summary>
        /// Сила атаки
        /// </summary>
        int Power
        { get; }

        /// <summary>
        /// Атаковать
        /// </summary>
        /// <param name="target">Цель атаки</param>
        /// <returns>true, если объект уничтожен атакой</returns>
        void Attack(IDestroyable target);

        /// <summary>
        /// Изменение здоровья объекта
        /// </summary>
        /// <param name="health">Количество изменяемых единиц здоровья</param>
        void DeltaPower(int delta);

        /// <summary>
        /// Событие, вызывающееся при изменении силы атаки
        /// </summary>
        event InGameEvent OnPowerChanged;
    }
}