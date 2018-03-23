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
        readonly Modifier[] modifiers;
        /// <summary>
        /// Модификаторы, содержащиеся в этой карте
        /// </summary>
        public Modifier[] Modifiers
        { get { return modifiers; } }

        int distance = 0;
        /// <summary>
        /// Дистанция воздействия. 0 => только на текущую клетку
        /// </summary>
        public int Distance
        {
            get { return distance; }
            protected set { if (value < 0) value = 0; distance = value; }
        }

        public SpellCard(int id, int cost, Rarity rarity, Modifier modifier)
            : base(id, cost, rarity)
        {
            if (modifier == null) throw new ArgumentNullException();
            modifiers = new Modifier[] { modifier };
        }

        public SpellCard(int id, int cost, Rarity rarity, Modifier[] modifiers)
            : base(id, cost, rarity)
        {
            if (modifiers == null) throw new ArgumentNullException();
            if (modifiers.Length == 0) throw new ArgumentException();
            modifiers.CopyTo(modifiers, 0);
        }

        public void Use(/*Position position*/)
        {
            //TODO
        }
    }
}
