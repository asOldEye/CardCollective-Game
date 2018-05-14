using System;
using System.Collections.Generic;

namespace CardCollectiveSession
{
    /// <summary>
    /// Контроллер сессии
    /// </summary>
    public class Controller
    {
        /// <summary>
        /// Сессия
        /// </summary>
        public Session Session { get; }
        /// <summary>
        /// Сделать ход
        /// </summary>
        public void MakeMove(int subjectID, Type componentType, string command, object[] param)
        {
            if (Session == null) throw new ArgumentException("Not in session");
            try
            { Session.MakeMove(this, componentType, subjectID, command, param); }
            catch { throw; }
        }

        /// <summary>
        /// Выгрузить сессию целиком
        /// </summary>
        public void UnloadWholeSession()
        {

        }

        /// <summary>
        /// Выгрузжать изменения в сессии
        /// </summary>
        public bool UnloadSessionChanges { get; set; }
        /// <summary>
        /// Объекты, принадлежащие этому хозяину
        /// </summary>
        public List<Container> OwnedObjects { get; } = new List<Container>();

        void OnSessionChanged()
        {

        }
    }
}