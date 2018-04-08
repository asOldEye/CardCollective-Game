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
        /// <summary>
        /// Модификаторы, содержащиеся в этой карте
        /// </summary>
        public List<Modifier> Modifiers { get; }

        /// <summary>
        /// Дистанция воздействия. 0 => только на текущую клетку
        /// </summary>
        public int Radius { get; } = 0;

        public SpellCard(Session session, int cost, Modifier modifier, int radius)
            : base(session, cost)
        {
            if (modifier == null) throw new ArgumentNullException();

            Modifiers = new List<Modifier> { modifier };

            Radius = radius;
        }

        public SpellCard(Session session, int cost, ICollection<Modifier> modifiers, int radius)
            : base(session, cost)
        {
            if (modifiers == null) throw new ArgumentNullException();
            if (modifiers.Count == 0) throw new ArgumentException();

            Modifiers = new List<Modifier>(modifiers);

            Radius = radius;
        }

        /// <summary>
        /// Использовать карту заклинания
        /// </summary>
        /// <param name="owner">владелец цели</param>
        /// <param name="target">цель</param>
        public void Use(Player owner, Position target)
        {
            if (owner == null) throw new ArgumentNullException();

            var f = owner.Map.ByRadius(target, Radius);

            foreach (var targ in f)
            {
                if (targ is IModified)
                {
                    foreach (var mod in Modifiers)
                    {
                        if (targ is IModifiedDurable)
                            (targ as IModifiedDurable).TakeModifier(mod);
                        else
                            (targ as IModified).TakeModifier(mod);
                    }
                }
            }
        }

        /// <summary>
        /// Использовать карту заклинания
        /// </summary>
        /// <param name="owner">владелец цели</param>
        /// <param name="target">цель</param>
        public void Use(Player owner, IModified target)
        {
            if (owner == null) throw new ArgumentNullException();
            if (target == null) throw new ArgumentNullException();

            if (target is IPositionable)
            {
                var f = owner.Map.ByRadius((target as IPositionable).Position, Radius);

                foreach (var targ in f)
                {
                    if (targ is IModified)
                    {
                        foreach (var mod in Modifiers)
                        {
                            if (targ is IModifiedDurable)
                                (targ as IModifiedDurable).TakeModifier(mod);
                            else
                                (targ as IModified).TakeModifier(mod);
                        }
                    }
                }
            }
            else
                foreach (var mod in Modifiers)
                    if (target is IModifiedDurable)
                        (target as IModifiedDurable).TakeModifier(mod);
                    else
                        (target as IModified).TakeModifier(mod);
        }
    }
}