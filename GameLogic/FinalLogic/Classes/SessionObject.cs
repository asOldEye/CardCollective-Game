namespace GameLogic
{
    /// <summary>
    /// Любой объект, находящийся в данной сессии
    /// </summary>
    public class SessionObject
    {
        Session session;
        /// <summary>
        /// Сессия, в  которой находится объект
        /// </summary>
        public Session Session
        { get { return session; } }

        public SessionObject(Session session)
        { this.session = session ?? throw new System.ArgumentNullException(); }
    }
}
