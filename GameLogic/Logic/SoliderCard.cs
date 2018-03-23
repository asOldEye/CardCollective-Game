using System;
using System.Collections.Generic;

namespace GameLogic
{
    /// <summary>
    /// Карта, представляющая существо
    /// </summary>
    public class SoliderCard : Card, IDestroyable, IAttacking, IModifiedObject<Modifier>
    {
        private int power = -1;
        /// <summary>
        /// Сила атаки
        /// </summary>
        public int Power
        {
            get { return power; }
            set { if (value < 0) throw new ArgumentException("Wrong power value"); power = value; }
        }

        private int health = -1;
        /// <summary>
        /// Параметр здоровья карты
        /// </summary>
        public int Health
        {
            get { return health; }
            set { if (value < 0) throw new ArgumentException("Wrong health value"); health = value; }
        }
        
        private int loyality = -1;
        /// <summary>
        /// Параметр лояльности карты
        /// </summary>
        public int Loyality
        {
            get { return loyality; }
            protected set { if (value < 0 || value > 100) throw new ArgumentException("Wrong loyality value"); loyality = value; }
        }
        
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
            int power, int health, int loyality, SoliderClass soliderClass)
            : base(id, cost, rarity)
        {
            try
            {
                Power = power;
                Health = health;
                Loyality = loyality;
            }
            catch (Exception e)
            { throw e; }

            this.soliderClass = soliderClass;
        }

        /// <summary>
        /// Атака уничтожаемого объекта
        /// </summary>
        /// <param name="target">атакуемый объект</param>
        /// <param name="modifier">модификатор урона</param>
        /// <returns></returns>
        public bool Attack(IDestroyable target)
        {
            if (target == null) throw new ArgumentException("Invalid target value");
            return false;
            //TODO
        }

        /// <summary>
        /// Получение урона
        /// </summary>
        /// <param name="damage">Получаемый урон</param>
        /// <returns></returns>
        public bool TakeDamage(int damage)
        {
            if (damage < 0) throw new ArgumentException("Invalid damage value");
            return false;
            //TODO
        }

        /// <summary>
        /// Лечение
        /// </summary>
        /// <param name="health">Восстанавливаемое здоровье</param>
        public void Cure(int health)
        {
            if (health < 0) throw new ArgumentException("Invalid health value");

            //TODO
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


        Queue<KeyValuePair<Modifier, int>> modifiers;
        public Queue<KeyValuePair<Modifier, int>> Modifiers
        { get { return modifiers; } }

        public void AddModifier(Modifier modifier, int time)
        {
            //TODO
        }

        public void TurnTick()
        {
            //TODO
        }
    }
}