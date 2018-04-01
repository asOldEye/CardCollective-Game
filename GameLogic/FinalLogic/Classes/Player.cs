using System;
using System.Collections.Generic;

namespace GameLogic
{
    /// <summary>
    /// Реализация игрока
    /// </summary>
    public class Player : SessionObject, IDestroyable, IAttacker, ICaster, IModified
    {
        public Player(Session session, 
            int health, int healthMax, int power, int powerMax, int mana, int manaMax,
            Deck<SpellCard> spells, Deck<SoliderCard> soliders, Map map) 
            : base(session)
        {
            Spells = spells;
            Soliders = soliders;

            Health = health;
            HealthMax = healthMax;

            Power = power;
            PowerMax = powerMax;

            Mana = mana;
            ManaMax = manaMax;

            Map = map;
        }

        /// <summary>
        /// Колода солдат игрока
        /// </summary>
        public Deck<SoliderCard> Soliders { get; }

        /// <summary>
        /// Предоставляет доступ к карте игрока
        /// </summary>
        public Map Map { get; }

        /// <summary>
        /// Расположенные карты
        /// </summary>
        public List<SoliderCard> LocatedCards;

        /// <summary>
        /// Вытаскивает карту солдата из колоды на стол
        /// </summary>
        /// <param name="solider"></param>
        public void CastSolider(SoliderCard solider, Position position)
        {
            try
            {
                if (Map.FindByPosition(position).Obj2 != null)
                    throw new ArgumentException("Position already used");
            }
            catch { throw; }

            Map.SetObj(solider, position);
            solider.OnDeath += OnSoliderDeath;

            if (OnSoliderCasted != null)
                OnSoliderCasted.Invoke(solider, new ObjSessionEventArgs(solider));
        }

        /// <summary>
        /// Вызывается при смерти одной из моих карт
        /// </summary>
        public void OnSoliderDeath(object sender, SessionEventArgs e)
        {
            LocatedCards.Remove(sender as SoliderCard);
        }

        /// <summary>
        /// Вызывается при вызове карты из колоды на стол
        /// </summary>
        public event InGameEvent OnSoliderCasted;

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

            target.DeltaHealth(-power);
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

        #region IModified realization
        /// <summary>
        /// Добавление нового модификатора
        /// </summary>
        /// <param name="modifier">Модификатор</param>
        public void TakeModifier(Modifier modifier)
        {
            if (modifier == null)
                throw new ArgumentNullException();

            modifier.Modified = this;
            modifier.Action();

            if (OnModifierUse != null)
                OnModifierUse.Invoke(this, new ModifierSessionEventArgs(modifier));
        }

        /// <summary>
        /// Вызывается при использовании модификатора
        /// </summary>
        public event InGameEvent OnModifierUse;
        #endregion

        #region ICaster realization
        /// <summary>
        /// Максимальное количество маны
        /// </summary>
        public int ManaMax { get; } = -1;

        int mana = -1;
        /// <summary>
        /// Текущее количество маны
        /// </summary>
        public int Mana
        {
            get { return mana; }
            set
            {
                if (value < 0) value = 0;
                if (value > ManaMax) value = ManaMax;

                Means means = mana > value ? Means.Positive : Means.Negative;

                mana = value;

                if (OnManaChanged != null)
                    OnManaChanged.Invoke(this, new SessionEventArgs(means, Context.mana));
            }
        }

        /// <summary>
        /// Колода заклинаний
        /// </summary>
        public Deck<SpellCard> Spells { get; }

        /// <summary>
        /// Создание заклинания
        /// </summary>
        /// <param name="spell">Заклинание</param>
        /// <param name="owner">Владелец цели</param>
        /// <param name="target">Цель</param>
        public void Cast(SpellCard spell, Player owner, Position target)
        {
            if (!Spells.RemoveCard(spell)) throw new ArgumentException("This is not my spell");
            if (spell.Cost > mana) throw new ArgumentException("Low mana");

            try
            {
                spell.Use(owner, target);
            }
            catch { throw; }

            Mana -= spell.Cost;

            if (OnSpellCast != null)
                OnSpellCast.Invoke(this, new ObjSessionEventArgs(spell));
        }

        /// <summary>
        /// Изменение количества маны
        /// </summary>
        /// <param name="delta">меняемое количество</param>
        public void DeltaMana(int delta)
        { Mana += delta; }

        /// <summary>
        /// Событие изменения количества маны
        /// </summary>
        public event InGameEvent OnManaChanged;
        /// <summary>
        /// Событие создания нового заклинания
        /// </summary>
        public event InGameEvent OnSpellCast;
        #endregion
    }
}