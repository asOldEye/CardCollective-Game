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

        public SpellCard(Session session, int id, int cost, Modifier modifier)
            : base(session, id, cost)
        {
            if (modifier == null) throw new ArgumentNullException();
            modifiers = new Modifier[] { modifier };
        }

        public SpellCard(Session session, int id, int cost, Modifier[] modifiers)
            : base(session, id, cost)
        {
            if (modifiers == null) throw new ArgumentNullException();
            if (modifiers.Length == 0) throw new ArgumentException();
            modifiers.CopyTo(this.modifiers, 0);
        }

        public void Use(IModified target)
        {
            if (target == null) throw new ArgumentNullException();
                foreach (var f in modifiers)
                    target.TakeModifier(new Modifier(f, target));
        }

        public void Use(Position target)
        {
            //TODO
        }
    }
}