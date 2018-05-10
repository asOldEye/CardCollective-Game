using AuxiliaryLibrary;

namespace Session
{
    /// <summary>
    /// Способный атаковать
    /// </summary>
    public interface IAttacker
    {
        /// <summary>
        /// Максимальная сила атаки
        /// </summary>
        int AttackPowerMax
        { get; }
        /// <summary>
        /// Сила атаки
        /// </summary>
        int AttackPower
        { get; }

        /// <summary>
        /// Атаковать
        /// </summary>
        /// <param name="target">Цель атаки</param>
        void Attack(IDestroyable target);

        /// <summary>
        /// Изменение здоровья объекта
        /// </summary>
        /// <param name="health">Количество изменяемых единиц здоровья</param>
        void DeltaPower(int delta);

        /// <summary>
        /// Событие, вызывающееся при изменении силы атаки
        /// </summary>
        event ParametrizedEventHandler<IAttacker, int> OnPowerChanged;
        /// <summary>
        /// Событие, вызывающееся при атаке
        /// </summary>
        event ParametrizedEventHandler<IAttacker, IDestroyable> OnAttack;
    }
}