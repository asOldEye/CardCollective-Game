namespace GameLogic
{
    /// <summary>
    /// Объект, находящийся в данной сессии
    /// </summary>
    public class SessionObject
    {
        /// <summary>
        /// Сессия, в  которой находится объект
        /// </summary>
        public readonly Session Session;

        public SessionObject(Session session)
        {
            if(session == null) throw new System.ArgumentNullException();
            Session = session;
        }
    }
}
