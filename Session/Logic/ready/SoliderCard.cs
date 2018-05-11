using AuxiliaryLibrary;
using System;
using System.Collections.Generic;

namespace CardCollectiveSession
{
    /// <summary>
    /// Карта, представляющая существо
    /// </summary>
    public class SoliderCard : PositionableSessionObject, IDestroyable, IAttacker, ILoyal, IMovable, ICard
    {
        #region ICard
        /// <summary>
        /// Стоимость вызова этой карты
        /// </summary>
        public int Cost { get; private set; }
        #endregion

        #region IAttacker realization
        /// <summary>
        /// Максимальная сила атаки
        /// </summary>
        public int AttackPowerMax { get; private set; }

        private int attackPower;
        /// <summary>
        /// Сила атаки
        /// </summary>
        public int AttackPower
        {
            get { return attackPower; }
            protected set
            {
                if (value < 1) value = 1;
                if (value > AttackPowerMax) value = AttackPowerMax;

                int delta = attackPower - value;
                attackPower = value;

                if (OnPowerChanged != null)
                    OnPowerChanged.Invoke(this, delta);
            }
        }

        /// <summary>
        /// Атака уничтожаемого объекта
        /// </summary>
        /// <param name="target">атакуемый объект</param>
        /// <returns></returns>
        public void Attack(IDestroyable target)
        {
            if (target == null) throw new ArgumentException("Invalid target value");

            if (OnAttack != null)
                OnAttack.Invoke(this, target);
            int attackPower = this.attackPower;

            if (target is SoliderCard)
                if (Session.Random.NextPercent(
                    Session.ClassesOpitions.
                    CritChance(SoliderClass, (target as SoliderCard).SoliderClass)))
                    attackPower = attackPower * 150 / 100;

            target.DeltaHealth(-attackPower);
        }

        /// <summary>
        /// Изменение здоровья объекта
        /// </summary>
        /// <param name="health">Количество изменяемых единиц здоровья</param>
        public virtual void DeltaPower(int delta)
        { AttackPower += delta; }

        /// <summary>
        /// Событие, вызывающееся при изменении силы атаки
        /// </summary>
        public event ParametrizedEventHandler<IAttacker, int> OnPowerChanged;
        /// <summary>
        /// Событие, вызывающееся при изменении силы атаки
        /// </summary>
        public event ParametrizedEventHandler<IAttacker, IDestroyable> OnAttack;
        #endregion

        #region IDestroyable realization
        /// <summary>
        /// Максимальное здоровье карты
        /// </summary>
        public int HealthMax { get; private set; }

        private int health;
        /// <summary>
        /// Параметр здоровья карты
        /// </summary>
        public int Health
        {
            get { return health; }
            protected set
            {
                if (value < 0) value = 0;
                if (value > HealthMax) value = HealthMax;

                int delta = health - value;

                if (OnHealthChanged != null)
                    OnHealthChanged.Invoke(this, delta);

                if ((health = value) == 0)
                    if (OnDeath != null) OnDeath.Invoke(this);
            }
        }

        /// <summary>
        /// Изменение здоровья объекта
        /// </summary>
        /// <param name="health">Количество изменяемых единиц здоровья</param>
        public virtual void DeltaHealth(int delta)
        { Health += delta; }

        /// <summary>
        /// Событие, вызывающееся при смерти
        /// </summary>
        public event NonParametrizedEventHandler<IDestroyable> OnDeath;

        /// <summary>
        /// Событие, вызывающееся при изменении количества здоровья
        /// </summary>
        public event ParametrizedEventHandler<IDestroyable, int> OnHealthChanged;
        #endregion

        #region ILoyal realization
        private int loyality;
        /// <summary>
        /// Параметр лояльности карты
        /// </summary>
        public int Loyality
        {
            get { return loyality; }
            protected set
            {
                if (value < 0) value = 0;
                if (value > 100) value = 100;


                int delta = loyality - value;
                loyality = value;

                if (OnLoyalityChanged != null)
                    OnLoyalityChanged.Invoke(this, delta);
            }
        }

        /// <summary>
        /// Изменение параметра лояльности
        /// </summary>
        /// <param name="delta"></param>
        public virtual void DeltaLoyality(int delta)
        { Loyality += delta; }

        /// <summary>
        /// Событие, вызывающееся при изменении количества здоровья
        /// </summary>
        public event ParametrizedEventHandler<ILoyal, int> OnLoyalityChanged;
        #endregion;

        /// <summary>
        /// Класс существа
        /// </summary>
        public string SoliderClass { get; private set; }

        public SoliderCard(int cost,
            int attackPowerMax, int attackPower,
            int healthMax, int health,
            int moveRadius,
            int loyality,
            string soliderClass,
            ICollection<DurableModifier> modifiers = null)
        {
            if ((Cost = cost) < 0) throw new ArgumentException("Wrong cost");

            try
            {
                AttackPowerMax = attackPowerMax;
                AttackPower = attackPower;

                HealthMax = healthMax;
                Health = health;

                Loyality = loyality;
                MoveRadius = moveRadius;
            }
            catch { throw; }

            SoliderClass = soliderClass;

            if (modifiers == null)
            {
                Modifiers = new List<DurableModifier>();
                return;
            }

            foreach (var f in modifiers)
                if (f == null) modifiers.Remove(f);
            Modifiers = new List<DurableModifier>(modifiers);
        }

        #region IModified realization
        /// <summary>
        /// Список модификаторов
        /// </summary>
        public List<DurableModifier> Modifiers
        { get; private set; } = new List<DurableModifier>();

        /// <summary>
        /// Добавляет модификатор
        /// </summary>
        public void AddModifier(DurableModifier modifier)
        {
            if (modifier == null) throw new ArgumentNullException("Null modifier");
            Modifiers.Add(modifier);
            if (OnModifierAdded != null)
                OnModifierAdded.Invoke(this as IModified, modifier);
        }
        /// <summary>
        /// Удаляет модификатор
        /// </summary>
        public void DelModifier(DurableModifier modifier)
        {
            if (modifier == null)
                throw new ArgumentNullException("Null modifier");
            if (!Modifiers.Remove(modifier))
                throw new ArgumentException("No this modifier in modifiers");
            if (OnModifierRemoved != null)
                OnModifierRemoved.Invoke(this as IModified, modifier);
        }

        /// <summary>
        /// Вызывается при добавлении модификатора
        /// </summary>
        event ParametrizedEventHandler<IModified, DurableModifier> OnModifierAdded;
        /// <summary>
        /// Вызывается при удалении модификатора
        /// </summary>
        event ParametrizedEventHandler<IModified, DurableModifier> OnModifierRemoved;
        #endregion

        #region IMovable realization
        int moveRadius;
        public int MoveRadius
        {
            get => moveRadius;
            protected set
            {
                if (value < 0) throw new ArgumentException("Move radius must be more than 0");
                int delta = moveRadius - (moveRadius = value);
                if (OnMoveRadiusChanged != null)
                    OnMoveRadiusChanged.Invoke(this, delta);
            }
        }
        public virtual void DeltaMoveRadius(int delta)
        { MoveRadius += delta; }

        public override void Move(Position position)
        {
            if (Position.Distance(position, Position) > MoveRadius)
                throw new ArgumentException("Can't move because too far");
            base.Move(position);
        }

        public event ParametrizedEventHandler<IMovable, int> OnMoveRadiusChanged;
        #endregion
    }
}