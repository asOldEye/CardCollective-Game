using System.Collections.Generic;

namespace GameLogic
{
    /// <summary>
    /// Солдат, создающий заклинания
    /// </summary>
    public class CasterSoliderCard : SoliderCard, ICaster
    {
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

                mana = value;

                if (OnManaChanged != null)
                    OnManaChanged.Invoke(this, new GameEventArgs(/*TODO*/));
            }
        }
        
        public CasterSoliderCard(int manaMax, int mana, Deck<SpellCard> spells, 
            int id, int cost, Rarity rarity, int power, int healthMax, int health, int loyality, SoliderClass soliderClass, List<Modifier> modifiers = null)
            : base(id, cost, rarity, power, healthMax, health, loyality, soliderClass, modifiers)
        {
            this.manaMax = manaMax;
            Mana = mana;
            this.spells = spells;
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
        public void Cast(SpellCard spell/*, Position target*/)
        {
            //TODO
        }

        public void DeltaMana(int delta)
        { Mana += delta; }
    }
}
