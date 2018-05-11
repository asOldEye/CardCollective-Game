using AuxiliaryLibrary;

namespace CardCollectiveSession
{
    /// <summary>
    /// Объект, находящийся в данной сессии
    /// </summary>
    public abstract class SessionObject
    {
        /// <summary>
        /// Сессия, в  которой находится объект
        /// </summary>
        public Session Session { get; private set; }
        /// <summary>
        /// Владелец этого объекта
        /// </summary>
        public Player Owner { get; private set; }

        public SessionObject(Session session, Player owner)
        {
            if ((Session = session) != null) Session.OnTurn += OnTurn;
            Owner = owner;
        }

        public SessionObject()
        {
            Session = null;
            Owner = null;
        }

        public T Clone<T>(T source) where T : SessionObject
        {
            return BinarySerializer.Deserialize(BinarySerializer.Serialize(source)) as T;
        }

        public void Attach(Session session, Player owner, Position position)
        {
            if (Session != null) Session.OnTurn -= OnTurn;
            if ((Session = session) != null)
            {
                Session.OnTurn += OnTurn;
                Session.AddSessionObject(this);
            }

            Owner = owner;
        }

        public virtual void Destroy()
        {
            Session.OnTurn -= OnTurn;
        }

        protected virtual void OnTurn(Player player) { }
    }
}