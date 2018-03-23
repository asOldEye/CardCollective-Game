using System;
using System.Collections.Generic;

namespace GameLogic
{
    /// <summary>
    /// Карта, представляющая существо
    /// </summary>
    public class SoliderCard : Card, IDestroyable, IAttacker, IModifiedDurable
    {
        #region IAttacker realization
        private int powerMax = -1;
        /// <summary>
        /// Максимальная сила атаки
        /// </summary>
        public int PowerMax
        {
            get { return powerMax; }
            protected set { if (value < 0) throw new ArgumentException(); }
        }

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
                power = value;

                if (OnPowerChanged != null)
                    OnPowerChanged.Invoke(this, new GameEventArgs(/*TODO*/));
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
        {
            Power += delta;
        }

        /// <summary>
        /// Событие, вызывающееся при изменении силы атаки
        /// </summary>
        public event InGameEvent OnPowerChanged;
        #endregion

        #region IDestroyable realization
        private int healthMax = -1;
        /// <summary>
        /// Максимальное здоровье карты
        /// </summary>
        public int HealthMax
        {
            get { return health; }
            protected set { if (value < 0) throw new ArgumentException("Wrong health value"); health = value; }
        }

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
                        OnDeath.Invoke(this, new GameEventArgs(/*TODO*/));

                    return;
                }
                if (value > healthMax) value = healthMax;

                health = value;

                if (OnHealthChanged != null)
                    OnHealthChanged.Invoke(this, new GameEventArgs(/*TODO*/));
            }
        }

        /// <summary>
        /// Изменение здоровья объекта
        /// </summary>
        /// <param name="health">Количество изменяемых единиц здоровья</param>
        public void DeltaHealth(int delta)
        {
            if (health > 0) Health += delta;

            //TODO
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
                loyality = value;
                if (OnLoyalityChanged != null) OnLoyalityChanged.Invoke(this, new GameEventArgs(/*TODO*/));
            }
        }

        /// <summary>
        /// Изменение параметра лояльности
        /// </summary>
        /// <param name="delta"></param>
        public void DeltaLoyality(int delta)
        {
            Loyality += delta;
        }
        
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
            int power, int healthMax, int health, int loyality, SoliderClass soliderClass, List<Modifier> modifiers = null)
            : base(id, cost, rarity)
        {
            Power = power;
            HealthMax = healthMax;
            Health = health;
            Loyality = loyality;
            SoliderClass = soliderClass;

            try
            {
                Modifiers = modifiers;
            }
            catch (Exception e) { throw e; }
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
        List<Modifier> modifiers;
        /// <summary>
        /// Список модификаторов
        /// </summary>
        public List<Modifier> Modifiers
        {
            get { return modifiers; }
            protected set
            {
                if (value == null) throw new ArgumentNullException();
                foreach (var f in value)
                    if (f == null) value.Remove(f);
                modifiers = new List<Modifier>(value);
            }
        }

        /// <summary>
        /// Добавление нового модификатора
        /// </summary>
        /// <param name="modifier">Модификатор</param>
        public void TakeModifier(Modifier modifier)
        {
            try
            {
                if (modifier is DurableModifier)
                    Modifiers.Add(modifier);
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
                f.Action();
        }
        #endregion
    }
}