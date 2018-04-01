using System;

namespace GameLogic
{
    /// <summary>
    /// Событийные аргументы внутри сессии
    /// </summary>
    public class SessionEventArgs : EventArgs
    {
        /// <summary>
        /// контекст события
        /// </summary>
        public readonly Context? context;

        /// <summary>
        /// значение события
        /// </summary>
        public readonly Means? means;

        public SessionEventArgs(Means? means = null, Context? context = null)
        {
            this.context = context;
            this.means = means;
        }
    }
    /// <summary>
    /// Событийные агрументы движения
    /// </summary>
    public class MoveSessionEventArgs : SessionEventArgs
    {
        public MoveSessionEventArgs(Position oldPos, Position newPos)
        {
            OldPos = oldPos;
            NewPos = newPos;
        }

        /// <summary>
        /// Старая позиция
        /// </summary>
        public Position OldPos { get; }
        /// <summary>
        /// Новая позиция
        /// </summary>
        public Position NewPos { get; }
    }
    /// <summary>
    /// Событийные агрументы взаимодействия модификаторов
    /// </summary>
    public class ModifierSessionEventArgs : SessionEventArgs
    {
        public ModifierSessionEventArgs(Modifier modifier)
        {
            if (modifier == null) throw new ArgumentNullException("Null modifier");
            Modifier = modifier;
        }

        /// <summary>
        /// Модификатор, у которого произошли изменения
        /// </summary>
        public Modifier Modifier { get; }
    }
    /// <summary>
    /// Событийные аргументы вызова на поле новых карт
    /// </summary>
    public class ObjSessionEventArgs : SessionEventArgs
    {
        public ObjSessionEventArgs(SessionObject obj)
        {
            if (obj == null) throw new ArgumentNullException("Null modifier");
            Obj = obj;
        }

        /// <summary>
        /// Вызванный объект
        /// </summary>
        public SessionObject Obj { get; }
    }
}