using AuxiliaryLibrary;
using System;

namespace CardCollectiveSession
{
    public class Walkable : Positionable
    {
        public Walkable(Position position, bool positioned, int moveRadius)
            : base(position, positioned)
        { MoveRadius = moveRadius; }

        int moveRadius;
        /// <summary>
        /// Радиус передвижения за 1 ход
        /// </summary>
        public int MoveRadius
        {
            get => moveRadius;
            protected set
            {
                if (value < 0) value = 0;
                int delta = moveRadius - (moveRadius = value);
                if (OnMoveRadiusChanged != null)
                    OnMoveRadiusChanged.Invoke(new SessionChange()); //TODO
            }
        }
        [Modified]
        public virtual void DeltaMoveRadius(int delta)
        { MoveRadius += delta; }

        /// <summary>
        /// Передвинуть на новую позицию
        /// </summary>
        /// <param name="position"></param>
        [Modified]
        [ControllerCommand(false)]
        public void Move(Position position)
        {
            if (Position.Distance(position, Position) > MoveRadius)
                throw new ArgumentException("Can't move because too far");
            Container.Session.Map.FindByPosition(Position).Positioned = null;
            Container.Session.Map.FindByPosition(position).Positioned = Container;
            if (OnPositionChanged != null)
                OnPositionChanged.Invoke(new SessionChange()); //TODO
        }

        public override void Appear(Container container)
        {
            base.Appear(container);
            OnMoveRadiusChanged += container.Session.SessionChanged;
            OnPositionChanged += container.Session.SessionChanged;
        }

        public event NonParametrizedEventHandler<SessionChange> OnMoveRadiusChanged;
        public event NonParametrizedEventHandler<SessionChange> OnPositionChanged;
    }
}