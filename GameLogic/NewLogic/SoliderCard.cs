using System;
using System.Collections.Generic;

namespace GameLogic
{
    /// <summary>
    /// Карта, представляющая существо
    /// </summary>
    public class SoliderCard : Card, IDestroyable, IAttacker, IModified
    {
        #region IAttacker realization
        private int power = -1;
        /// <summary>
        /// Сила атаки
        /// </summary>
        public int Power
        {
            get { return power; }
            protected set { if (value < 0) value = 0; power = value; }
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
        public event GameEvent AttackChanged;
        #endregion

        #region IDestroyable realization
        private int healthMax = -1;
        /// <summary>
        /// Параметр здоровья карты
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
            protected set { if (value < 0) value = 0; health = value; }
        }
        /// <summary>
        /// Получение урона
        /// </summary>
        /// <param name="damage">Получаемый урон</param>
        /// <returns></returns>
        public void TakeDamage(int damage)
        {
            if (damage < 0) throw new ArgumentException("Invalid damage value");
            //TODO
        }
        /// <summary>
        /// Изменение здоровья объекта
        /// </summary>
        /// <param name="health">Количество изменяемых единиц здоровья</param>
        public void DeltaHealth(int delta)
        {
            //TODO
        }
        /// <summary>
        /// Событие, вызывающееся при смерти
        /// </summary>
        public event GameEvent Death;
        /// <summary>
        /// Событие, вызывающееся при изменении количества здоровья
        /// </summary>
        public event GameEvent HealthChanged;
        #endregion

        #region Loyality realization
        private int loyality = -1;
        /// <summary>
        /// Параметр лояльности карты
        /// </summary>
        public int Loyality
        {
            get { return loyality; }
            protected set { if (value < 0) value = 0; if (value > 100) value = 100; loyality = value; }
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
        public event GameEvent LoyalityChanged;
        #endregion;

        private SoliderClass soliderClass;
        /// <summary>
        /// Класс существа
        /// </summary>
        public SoliderClass SoliderClass
        {
            get { return soliderClass; }
            private set { soliderClass = value; }
        }

        //констркутор
        public SoliderCard(int id, int cost, Rarity rarity,
            int power, int healthMax, int health, int loyality, SoliderClass soliderClass, List<Modifier> modifiers = null)
            : base(id, cost, rarity)
        {
            Power = power;
            HealthMax = healthMax;
            Health = health;
            Loyality = loyality;
            this.soliderClass = soliderClass;

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

                    if (soliderClass == f.soliderClass)
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
                    else return soliderClass > f.soliderClass ? 1 : 0;
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
        { Modifiers.Add(modifier); }

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