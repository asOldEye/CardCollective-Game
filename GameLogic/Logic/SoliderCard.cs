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
        readonly int powerMax = -1;
        /// <summary>
        /// Максимальная сила атаки
        /// </summary>
        public int PowerMax
        { get { return powerMax; } }

        private int power = -1;
        /// <summary>
        /// Сила атаки
        /// </summary>
        public int Power
        {
            get { return power; }
            protected set
            {
                if (value < 0) value = 0;
                if (value > powerMax) value = powerMax;

                var args = new GameEventArgs(power < value ? GameEventArgs.Means.Positive : GameEventArgs.Means.Negative, Context.power);

                power = value;

                if (OnPowerChanged != null)
                    OnPowerChanged.Invoke(this, args);
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

            //TODO
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
        #endregion

        #region IDestroyable realization
        readonly int healthMax = -1;
        /// <summary>
        /// Максимальное здоровье карты
        /// </summary>
        public int HealthMax
        { get { return health; } }

        private int health = -1;
        /// <summary>
        /// Параметр здоровья карты
        /// </summary>
        public int Health
        {
            get { return health; }
            protected set
            {
                if (value <= 0)
                {
                    value = 0;

                    if (OnDeath != null)
                        OnDeath.Invoke(this, new GameEventArgs(GameEventArgs.Means.Death));

                    return;
                }

                if (value > HealthMax) value = HealthMax;

                var args = new GameEventArgs(health < value ? GameEventArgs.Means.Positive : GameEventArgs.Means.Negative, Context.health);

                health = value;

                if (OnHealthChanged != null)
                    OnHealthChanged.Invoke(this, args);
            }
        }

        /// <summary>
        /// Изменение здоровья объекта
        /// </summary>
        /// <param name="health">Количество изменяемых единиц здоровья</param>
        public void DeltaHealth(int delta)
        {
            //TODO модификаторы, дебил
            Health += delta;
        }

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
        private int loyality = -1;
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

                var args = new GameEventArgs(loyality < value ? GameEventArgs.Means.Positive : GameEventArgs.Means.Negative, Context.loyality);

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
        public readonly SoliderClass SoliderClass;

        //констркутор
        public SoliderCard(int id, int cost, Rarity rarity,
            int powerMax, int power, int healthMax, int health, int loyality, SoliderClass soliderClass, ICollection<DurableModifier> modifiers = null)
            : base(id, cost, rarity)
        {
            this.powerMax = powerMax;
            Power = power;

            this.healthMax = healthMax;
            Health = health;

            Loyality = loyality;
            SoliderClass = soliderClass;

            if (modifiers == null)
            {
                this.modifiers = new List<DurableModifier>();
                return;
            }

            foreach (var f in modifiers)
                if (f == null) modifiers.Remove(f);
            this.modifiers = new List<DurableModifier>(modifiers);
        }

        //перегрузка метода сравнения
        public override int CompareTo(Card other)
        {
            int baseSol;
            try
            {
                baseSol = base.CompareTo(other);
            }
            catch (Exception e)
            { throw e; }

            if (baseSol == 0)
            {
                if (other is SoliderCard)
                {
                    var f = other as SoliderCard;

                    if (SoliderClass == f.SoliderClass)
                    {
                        if (health == f.health)
                        {
                            if (power == f.power)
                            {
                                if (loyality == f.loyality) return 0;
                                else return loyality > f.loyality ? 1 : 0;
                            }
                            else return power > f.power ? 1 : 0;
                        }
                        else return health > f.health ? 1 : 0;
                    }
                    else return SoliderClass > f.SoliderClass ? 1 : 0;
                }
                else return -1;
            }
            return baseSol;
        }

        #region IModified realization
        List<DurableModifier> modifiers;
        /// <summary>
        /// Список модификаторов
        /// </summary>
        public List<DurableModifier> Modifiers
        { get { return modifiers; } }

        /// <summary>
        /// Добавление нового модификатора
        /// </summary>
        /// <param name="modifier">Модификатор</param>
        public void TakeModifier(Modifier modifier)
        {
            try
            {
                if (modifier is DurableModifier)
                {
                    Modifiers.Add(modifier as DurableModifier);
                }
                else modifier.Action();
            }
            catch (Exception e)
            { throw e; }
        }

        /// <summary>
        /// Выполняет модификаторы, либо убирает их по истечении ходов
        /// </summary>
        public void TurnRun()
        {
            foreach (var f in modifiers)
            {
                f.Action();

                if (f.Timing == 0)
                    modifiers.Remove(f);
            }
        }
        #endregion

        #region IPositionable realization
        public event InGameEvent OnPositionAppears;

        public event InGameEvent OnPositionDisappears;

        public event InGameEvent OnPositionSet;

        Position? position;
        public Position? Position
        {
            get { return position; }
            set
            {
                // TODO допили это говно, оно мне не нравистя
                if (position == null && value != null)
                {
                    if (OnPositionAppears != null)
                        OnPositionAppears.Invoke(this, new GameEventArgs(GameEventArgs.Means.Appears));
                }
                else
                if (value != null)
                {
                    if (OnPositionSet != null)
                        OnPositionSet.Invoke(this, new GameEventArgs(GameEventArgs.Means.Position));
                }
                else if (OnPositionDisappears != null)
                    OnPositionDisappears.Invoke(this, new GameEventArgs(GameEventArgs.Means.Position));

                position = value;
            }
        }
        #endregion
    }
}