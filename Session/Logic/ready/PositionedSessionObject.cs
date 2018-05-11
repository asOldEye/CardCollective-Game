using AuxiliaryLibrary;
using System;

namespace CardCollectiveSession
{
    public abstract class PositionableSessionObject : SessionObject
    {
        Position position;
        /// <summary>
        /// Позиция объекта на карте
        /// </summary>
        public Position Position
        {
            get => position;
            set
            {
                if (value != position && Session != null)
                {
                    if (position.X.CompareTo(Session.Map.Size) > 0)
                        throw new ArgumentException("Too large position, than map size");

                    if (OnPositionChanged != null)
                        OnPositionChanged.Invoke(this,
                            new Pair<Position, Position>(position, value));

                }
                position = value;
            }
        }

        /// <summary>
        /// Передвинуть на заданную позиицию
        /// </summary>
        /// <param name="position">Новая позиция</param>
        public virtual void Move(Position position)
        {
            try
            {
                Session.Map.DelObj(Position);
                Session.Map.SetObj(this, position);
            }
            catch { throw; }
            if (OnPositionChanged != null)
                OnPositionChanged.Invoke(this, new Pair<Position, Position>(Position, Position = position));
        }

        public override void Destroy()
        {
            base.Destroy();
            Session.Map.DelObj(position);
        }

        public PositionableSessionObject() : base()
        { Position = Position.Empty; }
        public PositionableSessionObject(Session session, Player owner, Position position) : base(session, owner)
        {
            try
            { Position = position; }
            catch { throw; }
        }

        /// <summary>
        /// Вызывается при изменении позиции
        /// </summary>
        public event ParametrizedEventHandler<SessionObject, Pair<Position, Position>> OnPositionChanged;
    }
}