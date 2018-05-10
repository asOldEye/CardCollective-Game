using System;
using System.Collections.Generic;

namespace Session
{
    /// <summary>
    /// Карта заклинания
    /// </summary>
    public class SpellCard : SessionObject, ICard
    {
        #region ICard
        /// <summary>
        /// Стоимость вызова этой карты
        /// </summary>
        public int Cost { get; private set; }
        #endregion

        /// <summary>
        /// Модификаторы, содержащиеся в этой карте
        /// </summary>
        public List<Modifier> Modifiers
        { get; }

        int radius;
        /// <summary>
        /// Дистанция воздействия. 0 => только на текущую клетку
        /// </summary>
        public int Radius
        {
            get => radius;
            protected set
            {
                if ((radius = value) < 0)
                    throw new ArgumentException("Radius must be equals or more then 0");
            }
        }
        
        public SpellCard(int cost, ICollection<Modifier> modifiers, int radius)
        {
            if (modifiers == null) throw new ArgumentNullException("Modifiers is null");
            if (modifiers.Count == 0) throw new ArgumentException("Modifiers's count = 0");

            Modifiers = new List<Modifier>(modifiers);
            foreach (var f in Modifiers)
                if (f == null) throw new ArgumentException("Modifierscan't contain null values");

            try
            { Radius = radius; }
            catch { throw; }
        }

        /// <summary>
        /// Использовать карту заклинания
        /// </summary>
        /// <param name="owner">владелец цели</param>
        /// <param name="target">цель</param>
        public void Use(Position target)
        {
            var dots = Session.Map.ByRadius(target, radius);

            List<PositionableSessionObject> targets = new List<PositionableSessionObject>();
            foreach (var f in dots)
                if (f.Positioned != null)
                    targets.Add(f.Positioned as PositionableSessionObject);

            foreach (var targ in targets)
                foreach (var f in Modifiers)
                    if (f is DurableModifier)
                    {
                        if (targ is IModified)
                            (f as DurableModifier).AddModified(targ as IModified);
                    }
                    else f.Action(targ);
            
        }
        public void Use(SessionObject target)
        {
            if (target is PositionableSessionObject)
                Use(Session.Map.FindPosition(target as PositionableSessionObject));
            else
            {
                foreach (var f in Modifiers)
                    if (f is DurableModifier)
                    {
                        if (target is IModified)
                            (f as DurableModifier).AddModified(target as IModified);
                    }
                    else f.Action(target);
            }
        }
    }
}