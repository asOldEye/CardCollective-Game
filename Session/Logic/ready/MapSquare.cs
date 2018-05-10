using System.Collections.Generic;

namespace Session
{
    /// <summary>
    /// Квадрат карты
    /// </summary>
    public class MapSquare
    {
        internal MapSquare(List<Modifier> modifiers, PositionableSessionObject positioned)
        {
            if ((Modifiers = modifiers) == null) Modifiers = new List<Modifier>();
            Positioned = positioned;
        }

        /// <summary>
        /// Модификатор на данной клетке
        /// </summary>
        public virtual List<Modifier> Modifiers
        { get; }
        /// <summary>
        /// Объект, расположенный на этой клетке
        /// </summary>
        public virtual PositionableSessionObject Positioned
        { get; set; }
    }
}
