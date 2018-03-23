using System;
using System.Collections.Generic;
using System.Collections;

namespace GameLogic
{
    /// <summary>
    /// Реализация игрока
    /// </summary>
    public class Player : IDestroyable, IAttacker, IModified, ICaster
    {
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
        public event InGameEvent Death;
        /// <summary>
        /// Событие, вызывающееся при изменении количества здоровья
        /// </summary>
        public event InGameEvent HealthChanged;
        #endregion

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
        public event InGameEvent AttackChanged;
        #endregion

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

        #region ICaster realization
        int mana = -1;
        /// <summary>
        /// Количество маны
        /// </summary>
        public int Mana
        {
            get { return mana; }
            protected set
            {
                if (value < 0) value = 0; mana = value;
            }
        }
        /// <summary>
        /// Изменение здоровья объекта
        /// </summary>
        /// <param name="health">Количество изменяемых единиц здоровья</param>
        public void DeltaMana(int delta)
        {
            Mana += delta;
        }

        /// <summary>
        /// Создать заклинание
        /// </summary>
        /// <param name="spell">Заклинание из списка доступных</param>
        /// <param name="target">Цель атаки</param>
        public void Cast(SpellCard spell, IDestroyable target = null)
        {

        }

        Deck<SpellCard> spells;
        /// <summary>
        /// Список доступных заклинаний
        /// </summary>
        public Deck<SpellCard> Spells
        {
            get { return spells; }
            protected set
            {
                foreach (var f in value)
                    if (f == null) value.Remove(f);
                spells = value;
            }
        }

        /// <summary>
        /// Событие, вызывающееся при изменении силы атаки
        /// </summary>
        public event InGameEvent ManaChanged;
        #endregion

        //констркутор
        public Player(int id, int cost, Rarity rarity,
            int power, int healthMax, int health, List<Modifier> modifiers = null)
        {
            Power = power;
            HealthMax = healthMax;
            Health = health;

            try
            {
                Modifiers = modifiers;
            }
            catch (Exception e) { throw e; }
        }
    }
}