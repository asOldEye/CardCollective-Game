using System;
using System.Collections.Generic;

namespace GameLogic
{
    /// <summary>
    /// Карта, представляющая существо
    /// </summary>
    public class SoliderCard : Card, IDestroyable, IAttacker, IModifiedDurable, ILoyal, IPositionable
    {
        #region IAttacker realization
        /// <summary>
        /// Максимальная сила атаки
        /// </summary>
        public int PowerMax { get; }

        private int power;
        /// <summary>
        /// Сила атаки
        /// </summary>
        public int Power
        {
            get { return power; }
            protected set
            {
                if (value < 1) value = 1;
                if (value > PowerMax) value = PowerMax;

                Means means = power > value ? Means.Positive : Means.Negative;

                power = value;

                if (OnPowerChanged != null)
                    OnPowerChanged.Invoke(this, new SessionEventArgs(means, Context.power));
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
                OnAttack.Invoke(this, new ObjSessionEventArgs(target as SessionObject));

            int attack = power / 2 + power / 100 * loyality;

            if (target is SoliderCard)
                attack *= Session.SoliderClassesAttackMatrix[(int)soliderClass, (int)((target as SoliderCard).soliderClass)] / 100;

            target.DeltaHealth(-attack);
        }

        /// <summary>
        /// Изменение здоровья объекта
        /// </summary>
        /// <param name="health">Количество изменяемых единиц здоровья</param>
        public void DeltaPower(int delta)
        { Power += delta; }

        /// <summary>
        /// Событие, вызывающееся при изменении силы атаки
        /// </summary>
        public event InGameEvent OnPowerChanged;
        /// <summary>
        /// Событие, вызывающееся при изменении силы атаки
        /// </summary>
        public event InGameEvent OnAttack;
        #endregion

        #region IDestroyable realization
        /// <summary>
        /// Максимальное здоровье карты
        /// </summary>
        public int HealthMax { get; }

        private int health;
        /// <summary>
        /// Параметр здоровья карты
        /// </summary>
        public int Health
        {
            get { return health; }
            protected set
            {
                Means means = health > value ? Means.Positive : Means.Negative;

                if (value < 0)
                {
                    health = 0;

                    if (OnDeath != null)
                        OnDeath.Invoke(this, new SessionEventArgs(Means.Negative, Context.health));
                    return;
                }
                if (value > HealthMax) value = HealthMax;

                health = value;

                if (OnHealthChanged != null)
                    OnHealthChanged.Invoke(this, new SessionEventArgs(means, Context.health));
            }
        }

        /// <summary>
        /// Изменение здоровья объекта
        /// </summary>
        /// <param name="health">Количество изменяемых единиц здоровья</param>
        public void DeltaHealth(int delta)
        { Health += delta; }

        /// <summary>
        /// Событие, вызывающееся при смерти
        /// </summary>
        public event InGameEvent OnDeath;

        /// <summary>
        /// Событие, вызывающееся при изменении количества здоровья
        /// </summary>
        public event InGameEvent OnHealthChanged;
        #endregion

        #region Loyality realization
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

                var args = new SessionEventArgs(loyality < value ? Means.Positive : Means.Negative, Context.loyality);

                loyality = value;

                if (OnLoyalityChanged != null)
                    OnLoyalityChanged.Invoke(this, args);
            }
        }

        /// <summary>
        /// Изменение параметра лояльности
        /// </summary>
        /// <param name="delta"></param>
        public void DeltaLoyality(int delta)
        { Loyality += delta; }

        /// <summary>
        /// Событие, вызывающееся при изменении количества здоровья
        /// </summary>
        public event InGameEvent OnLoyalityChanged;
        #endregion;

        /// <summary>
        /// Класс существа
        /// </summary>
        public readonly SoliderClass soliderClass;

        //констркутор
        public SoliderCard(Session session, int cost,
            int powerMax, int power, int healthMax, int health, int loyality, SoliderClass soliderClass,
            ICollection<DurableModifier> modifiers = null)
            : base(session, cost)
        {
            PowerMax = powerMax;
            Power = power;

            HealthMax = healthMax;
            Health = health;

            Loyality = loyality;
            this.soliderClass = soliderClass;

            if (modifiers == null)
            {
                Modifiers = new List<DurableModifier>();
                return;
            }

            foreach (var f in modifiers)
                if (f == null) modifiers.Remove(f);
            Modifiers = new List<DurableModifier>(modifiers);
        }

        //перегрузка метода сравнения
        public override int CompareTo(Card other)
        {
            if (other is SoliderCard)
            {
                var baseSol = base.CompareTo(other);
                if (baseSol != 0) return baseSol;

                var f = other as SoliderCard;
                if (soliderClass == f.soliderClass)
                {
                    if (HealthMax == f.HealthMax)
                    {
                        if (PowerMax == f.PowerMax)
                        {
                            if (Loyality == f.Loyality) return 0;
                        }
                        return PowerMax > f.PowerMax ? 1 : 0;
                    }
                    return PowerMax > f.HealthMax ? 1 : 0;
                }
                return soliderClass > f.soliderClass ? 1 : 0;
            }
            return -1;
        }

        #region IModified realization
        /// <summary>
        /// Список модификаторов
        /// </summary>
        public List<DurableModifier> Modifiers { get; }

        /// <summary>
        /// Добавление нового модификатора
        /// </summary>
        /// <param name="modifier">Модификатор</param>
        public void TakeModifier(Modifier modifier)
        {
            if (modifier == null) throw new ArgumentNullException();

            modifier.Modified = this;

            if (modifier is DurableModifier)
            {
                Modifiers.Add(modifier as DurableModifier);

                if (OnModifierAdd != null)
                    OnModifierAdd.Invoke(this, new ModifierSessionEventArgs(modifier));
            }
            else
            {
                modifier.Action();
                if (OnModifierUse != null)
                    OnModifierUse.Invoke(this, new ModifierSessionEventArgs(modifier));
            }
        }

        /// <summary>
        /// Удаление модификатора
        /// </summary>
        /// <param name="modifier"></param>
        public void DelModifier(DurableModifier modifier)
        {
            try
            {
                if (Modifiers.Remove(modifier))
                {
                    modifier.Modified = null;

                    if (OnModifierRemove != null)
                        OnModifierRemove.Invoke(this, new ModifierSessionEventArgs(modifier));
                }
                else throw new ArgumentException("Not my modifier");
            }
            catch { throw; }
        }

        /// <summary>
        /// Вызывается при использовании модификатора
        /// </summary>
        public event InGameEvent OnModifierUse;
        /// <summary>
        /// Вызывается при добавлении модификатора
        /// </summary>
        public event InGameEvent OnModifierAdd;
        /// <summary>
        /// Вызывается при удалении модификатора
        /// </summary>
        public event InGameEvent OnModifierRemove;

        /// <summary>
        /// Выполняет модификаторы, либо убирает их по истечении ходов
        /// </summary>
        public void TurnRun()
        {
            foreach (var f in Modifiers)
            {
                f.Action();
                if (f.Timing == 0)
                    Modifiers.Remove(f);
            }
        }
        #endregion

        #region IPositionable realization
        Position position;
        /// <summary>
        /// Позиция на карте
        /// </summary>
        public Position Position
        {
            get { return position; }
            set
            {
                var args = new MoveSessionEventArgs(position, value);

                position = value;

                if (OnPositionChanged != null)
                    OnPositionChanged.Invoke(this, args);
            }
        }

        public event InGameEvent OnPositionChanged;
        #endregion
    }
}