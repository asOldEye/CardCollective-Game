using System;
using System.Collections.Generic;

namespace CardCollectiveSession
{
    /// <summary>
    /// Квадрат карты
    /// </summary>
    class MapSquare
    {
        Position mapSize;

        internal MapSquare(Position mapSize, List<Modifier> modifiers, Container positioned)
        {
            this.mapSize = mapSize;
            if ((this.modifiers = modifiers) == null) this.modifiers = new List<Modifier>();
            Positioned = positioned;
        }

        List<Modifier> modifiers;
        /// <summary>
        /// Модификаторы на данной клетке
        /// </summary>
        public virtual Modifier[] Modifiers { get => modifiers.ToArray(); }
        /// <summary>
        /// Добавить модификатор
        /// </summary>
        public void AddModifier(Modifier mod)
        {
            if (mod == null) throw new ArgumentNullException(nameof(mod));
            if(modifiers.Contains(mod)) throw new ArgumentNullException(nameof(mod));
            modifiers.Add(mod);
            if (positioned != null)
                if (mod is DurableModifier)
                    (mod as DurableModifier).SetTiming(positioned, 0);
        }
        /// <summary>
        /// Удалить модификатор
        /// </summary>
        public bool DelModifier(Modifier mod)
        {
            if (positioned != null)
                if (mod is DurableModifier)
                    (mod as DurableModifier).SetTiming(positioned, 0);
            return modifiers.Remove(mod);
        }

        Container positioned;
        /// <summary>
        /// Объект, расположенный на этой клетке
        /// </summary>
        public virtual Container Positioned
        {
            get => positioned;
            set
            {
                if (value != null)
                {
                    if (value.GetComponent<Positionable>() == null)
                        throw new ArgumentException("Positioned container must contain 'positionable' component to be positioned on map");
                    if (positioned != null)
                        throw new ArgumentException("In this square already positioned another container");
                    foreach (var f in Modifiers)
                        f.Action(positioned = value);
                }
                else if (positioned != null)
                    foreach (var f in Modifiers)
                        if (f is DurableModifier)
                            (f as DurableModifier).SetTiming(positioned, 0);
                positioned = value;
            }
        }
    }
}