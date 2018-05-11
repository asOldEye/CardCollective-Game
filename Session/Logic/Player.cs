using AuxiliaryLibrary;
using System;

namespace CardCollectiveSession
{
    /// <summary>
    /// Реализация игрока
    /// </summary>
    public class Player : SessionObject, IDestroyable, IAttacker, ICaster
    {
        public Player(Session session,
            int health, int healthMax,
            int power, int powerMax,
            int mana, int manaMax,
            Deck<SpellCard> spells,
            Deck<SoliderCard> soliders)
            : base(session, null)
        {
            Spells = spells;
            Soliders = soliders;

            Health = health;
            HealthMax = healthMax;

            AttackPower = power;
            AttackPowerMax = powerMax;

            Mana = mana;
            ManaMax = manaMax;
        }

        /// <summary>
        /// Колода солдат игрока
        /// </summary>
        public Deck<SoliderCard> Soliders
        { get; } = new Deck<SoliderCard>();

        /// <summary>
        /// Вытаскивает карту солдата из колоды на стол
        /// </summary>
        /// <param name="solider"></param>
        public void CastSolider(SoliderCard solider, Position position)
        {
            if (!Soliders.RemoveCard(solider))
                throw new ArgumentException("Wrong solider");
            try
            {
                if (Session.Map.FindByPosition(position).Positioned != null)
                    throw new ArgumentException("Position already used");
            }
            catch { throw; }

            if (Mana < solider.Cost) throw new ArgumentException("Low mana");

            Mana -= solider.Cost;
            Session.Map.SetObj(solider, position);

            if (OnSoliderCasted != null)
                OnSoliderCasted.Invoke(this, position);
        }

        /// <summary>
        /// Вызывается при вызове карты из колоды на стол
        /// </summary>
        public event ParametrizedEventHandler<Player, Position> OnSoliderCasted;

        #region IAttacker realization
        /// <summary>
        /// Максимальная сила атаки
        /// </summary>
        public int AttackPowerMax { get; }

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

        #region ICaster realization
        /// <summary>
        /// Максимальное количество маны
        /// </summary>
        public int ManaMax
        { get; }

        int mana;
        /// <summary>
        /// Текущее количество маны
        /// </summary>
        public int Mana
        {
            get => mana;
            private set
            {
                if (value < 0) value = 0;
                if (value > ManaMax) value = ManaMax;

                int delta = value - mana;
                mana = value;

                if (OnManaChanged != null)
                    OnManaChanged.Invoke(this, delta);
            }
        }

        /// <summary>
        /// Колода заклинаний
        /// </summary>
        public Deck<SpellCard> Spells
        { get; } = new Deck<SpellCard>();

        /// <summary>
        /// Создание заклинания
        /// </summary>
        /// <param name="spell">Заклинание</param>
        /// <param name="target">Цель</param>
        public void Cast(SpellCard spell, Position target)
        {
            if (spell == null) throw new ArgumentNullException("Spell card is null");
            if (target.CompareTo(Session.Map.Size) >= 0) throw new ArgumentNullException("Target is null");

            if (!Spells.Contains(spell))
                throw new ArgumentException("It is'nt my spell");

            if (Mana < spell.Cost) throw new ArgumentException("Low mana");

            Mana -= spell.Cost;
            spell.Use(target);

            if (OnSpellCast != null)
                OnSpellCast.Invoke(this as ICaster, spell);
        }
        /// <summary>
        /// Создать заклинание
        /// </summary>
        /// <param name="spell">Заклинание</param>
        /// <param name="target">Цель атаки</param>
        public void Cast(SpellCard spell, SessionObject target)
        {
            if (spell == null) throw new ArgumentNullException("Spell card is null");
            if (target == null) throw new ArgumentNullException("Target is null");

            if (!Spells.Contains(spell))
                throw new ArgumentException("It is'nt my spell");

            if (Mana < spell.Cost) throw new ArgumentException("Low mana");

            Mana -= spell.Cost;
            if (target is PositionableSessionObject)
                spell.Use(Session.Map.FindPosition(target as PositionableSessionObject));
            else
                spell.Use(target);

            if (OnSpellCast != null)
                OnSpellCast.Invoke(this as ICaster, spell);
        }

        /// <summary>
        /// Изменение количества маны
        /// </summary>
        /// <param name="delta">меняемое количество</param>
        public virtual void DeltaMana(int delta)
        { Mana += delta; }

        /// <summary>
        /// Событие, вызывающееся при изменении количества маны
        /// </summary>
        public event ParametrizedEventHandler<ICaster, int> OnManaChanged;
        /// <summary>
        /// Событие вызывается при создании карты-заклинания
        /// </summary>
        public event ParametrizedEventHandler<ICaster, SpellCard> OnSpellCast;
        #endregion
    }
}