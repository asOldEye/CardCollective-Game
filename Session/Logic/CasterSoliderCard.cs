using System.Collections.Generic;
using System;
using AuxiliaryLibrary;

namespace CardCollectiveSession
{
    /// <summary>
    /// Солдат, создающий заклинания
    /// </summary>
    public class CasterSoliderCard : SoliderCard, ICaster
    {
        public CasterSoliderCard(int cost,
            int powerMax, int power, int healthMax, int health, int loyality, int moveRadius, string soliderClass,
            int manaMax, int mana, Deck<SpellCard> spells,
            List<DurableModifier> modifiers)
            : base(cost, powerMax, power, healthMax, health, loyality, moveRadius, soliderClass, modifiers)
        {
            ManaMax = manaMax;
            Mana = mana;
            Spells = spells;
        }

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
        /// Создать заклинание
        /// </summary>
        /// <param name="spell">Заклинание</param>
        /// <param name="target">Цель</param>
        public void Cast(SpellCard spell, Position target)
        {
            if (spell == null) throw new ArgumentNullException("Spell card is null");
            if (target.CompareTo(Session.Map.Size) >= 0) throw new ArgumentNullException("Target is null");

            if (!Spells.Contains(spell))
                throw new ArgumentException("It is'not my spell");
            
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
        /// <param name="target">Цель</param>
        public void Cast(SpellCard spell, SessionObject target)
        {
            if (spell == null) throw new ArgumentNullException("Spell card is null");
            if (target == null) throw new ArgumentNullException("Target is null");
            
            if (!Spells.Contains(spell))
                throw new ArgumentException("It is'nt my spell");

            if (Mana < spell.Cost) throw new ArgumentException("Low mana");

            Mana -= spell.Cost;
            spell.Use(target);

            if (OnSpellCast != null)
                OnSpellCast.Invoke(this as ICaster, spell);
        }

        /// <summary>
        /// Изменение количества маны
        /// </summary>
        /// <param name="delta">Меняемое количество</param>
        public void DeltaMana(int delta)
        { Mana += delta; }

        /// <summary>
        /// Событие, вызывающееся при изменении силы атаки
        /// </summary>
        public event ParametrizedEventHandler<ICaster, int> OnManaChanged;
        /// <summary>
        /// Событие вызывается при создании карты-заклинания
        /// </summary>
        public event ParametrizedEventHandler<ICaster, SpellCard> OnSpellCast;
        #endregion
    }
}