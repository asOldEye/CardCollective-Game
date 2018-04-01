using System.Collections.Generic;
using System;

namespace GameLogic
{
    /// <summary>
    /// Солдат, создающий заклинания
    /// </summary>
    public class CasterSoliderCard : SoliderCard, ICaster
    {
        public CasterSoliderCard(Session session, int cost,
            int powerMax, int power, int healthMax, int health, int loyality,
            int manaMax, int mana, Deck<SpellCard> spells,
            List<DurableModifier> modifiers = null)
            : base(session, cost, powerMax, power, healthMax, health, loyality, SoliderClass.circle, modifiers)
        {
            ManaMax = manaMax;
            Mana = mana;
            Spells = spells;
        }

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

        public override int CompareTo(Card other)
        {
            if (other is CasterSoliderCard)
            {
                var f = other as CasterSoliderCard;
                if (ManaMax == f.ManaMax)
                {
                    if (Spells.Count == f.Spells.Count)
                        return 0;
                    return Spells.Count > f.Spells.Count ? 1 : -1;
                }
                return ManaMax > f.ManaMax ? 1 : -1;
            }
            return base.CompareTo(other);
        }
    }
}