using System;
using AuxiliaryLibrary;

namespace CardCollectiveSession
{
    /// <summary>
    /// Располагаемый на карте объект
    /// </summary>
    public class Positionable : Component
    {
        bool positioned;
        /// <summary>
        /// Расположен
        /// </summary>
        public bool Positioned
        {
            get => positioned;
            set
            {
                if (value && !positioned)
                {
                    try
                    { Container.Session.Map.FindByPosition(Position).Positioned = Container; }
                    catch { throw; }
                    OnPositioned.Invoke(new SessionChange()); //TODO
                }
                else if (!value && positioned)
                    try
                    { Container.Session.Map.FindByPosition(Position).Positioned = null; }
                    catch { throw; }
                positioned = value;
            }
        }

        Position position;
        /// <summary>
        /// Позиция объекта на карте
        /// </summary>
        public Position Position
        {
            get => position;
            set
            {
                if (value.CompareTo(position) != 0)
                {
                    if (position.X.CompareTo(Container.Session.Map.Size) > 0)
                        throw new ArgumentException("Too large position, than map size");
                    position = value;
                }
            }
        }

        public override void Appear(Container container)
        {
            base.Appear(container);
            OnPositioned += container.Session.SessionChanged;
        }
        public override void Wanish()
        {
            base.Wanish();
            Container.Session.Map.FindByPosition(position).Positioned = null;
            Positioned = false;
        }
        public Positionable(Position position, bool positioned)
        {
            try
            {
                Position = position;
                Positioned = positioned;
            }
            catch { throw; }
        }

        public event NonParametrizedEventHandler<SessionChange> OnPositioned;
    }
}