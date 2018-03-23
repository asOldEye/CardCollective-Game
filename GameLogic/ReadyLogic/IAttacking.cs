namespace GameLogic
{
    /// <summary>
    /// Способный атаковать
    /// </summary>
    public interface IAttacking
    {
        /// <summary>
        /// Сила атаки
        /// </summary>
        int Power
        { get; set; }

        /// <summary>
        /// Атаковать
        /// </summary>
        /// <param name="target">Цель атаки</param>
        /// <returns>true, если объект уничтожен атакой</returns>
        bool Attack(IDestroyable target);
    }
}
