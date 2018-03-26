using System.Collections.Generic;

namespace GameLogic
{
    /// <summary>
    /// Солдат, создающий заклинания
    /// </summary>
    public class CasterSoliderCard : SoliderCard, ICaster
    {
        public CasterSoliderCard(int manaMax, int mana, Deck<SpellCard> spells,
            int id, int cost, Rarity rarity, int powerMax, int power, int healthMax, int health, int loyality, SoliderClass soliderClass, List<DurableModifier> modifiers = null)
            : base(id, cost, rarity, powerMax, power, healthMax, health, loyality, soliderClass, modifiers)
        {
            this.manaMax = manaMax;
            Mana = mana;
            this.spells = spells;
        }

        readonly int manaMax = -1;
        /// <summary>
        /// Максимальное количество маны
        /// </summary>
        public int ManaMax
        { get { return manaMax; } }

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

                var args = new GameEventArgs(mana < value ? GameEventArgs.Means.Positive : GameEventArgs.Means.Negative, Context.mana);

                mana = value;

                if (OnManaChanged != null)
                    OnManaChanged.Invoke(this, args);
            }
        }

        public readonly Deck<SpellCard> spells;
        /// <summary>
        /// Колода заклинаний
        /// </summary>
        public Deck<SpellCard> Spells
        { get { return spells; } }

        /// <summary>
        /// Событие изменения количества маны
        /// </summary>
        public event InGameEvent OnManaChanged;

        /// <summary>
        /// Создание заклинания
        /// </summary>
        /// <param name="spell">Заклинание</param>
        /// /// <param name="target">Позиция на доске, к которой применяется заклинание</param>
        public void Cast(SpellCard spell, Position target)
        {
            if (!spells.RemoveCard(spell)) throw new System.ArgumentException("This is not my spell");

            Mana -= spell.Cost;

            //TODO
        }

        /// <summary>
        /// Создание заклинания
        /// </summary>
        /// <param name="spell">Заклинание</param>
        /// /// <param name="target">Позиция на доске, к которой применяется заклинание</param>
        public void Cast(SpellCard spell, IPositionable target)
        {
            if (!spells.RemoveCard(spell)) throw new System.ArgumentException("This is not my spell");

            Mana -= spell.Cost;

            //TODO
        }

        //TODO сделай так, чтобы мы могли все объекты посмотреть на сцене, и чтобы они все могли подняться к сессии и работать с ней непосредственно

        /// <summary>
        /// Изменение количества маны
        /// </summary>
        /// <param name="delta"></param>
        public void DeltaMana(int delta)
        { Mana += delta; }
    }
}