using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    /// <summary>
    /// Карта заклинания
    /// </summary>
    public class SpellCard : Card
    {
        public SpellCard(int id, int cost, Rarity rarity, Modifier modifier)
            : base(id, cost, rarity)
        {

        }

        public SpellCard(int id, int cost, Rarity rarity, Modifier[] modifiers)
            : base(id, cost, rarity)
        {

        }
    }
}
