using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic.Logic
{
    class CasterSoliderCard : SoliderCard, ICaster
    {
        public CasterSoliderCard(int id, int cost, Rarity rarity, int power, int healthMax, int health, int loyality, SoliderClass soliderClass, List<Modifier> modifiers = null) : base(id, cost, rarity, power, healthMax, health, loyality, soliderClass, modifiers)
        {
        }

        public int Mana => throw new NotImplementedException();

        public Deck<SpellCard> Spells => throw new NotImplementedException();

        public event InGameEvent ManaChanged;

        public void Cast(SpellCard spell, IDestroyable target = null)
        {
            throw new NotImplementedException();
        }

        public void DeltaMana(int delta)
        {
            throw new NotImplementedException();
        }
    }
}
