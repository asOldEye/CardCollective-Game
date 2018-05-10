using System;
using System.Collections.Generic;

namespace Session
{
    /// <summary>
    /// Модификатор
    /// </summary>
    public class Modifier : SessionObject
    {
        /// <summary>
        /// Сила воздействия модификатора
        /// </summary>
        public int Impact
        { get; protected set; }
        protected readonly string deltaMethodName;

        public Modifier(Session session, int impact, string deltaMethodName) : base(session, null)
        {
            if ((this.deltaMethodName = deltaMethodName) == null)
                throw new ArgumentNullException("Null method name");
            Impact = impact;
        }

        /// <summary>
        /// Воздействие на модифицируемые объекты
        /// </summary>
        public virtual void Action(List<SessionObject> modified)
        {
            try
            {
                foreach (var f in modified) Action(f);
            }
            catch { throw; }
        }

        /// <summary>
        /// Воздействие на модифицируемый объект
        /// </summary>
        public virtual void Action(SessionObject target)
        {
            try
            {
                var f = target.GetType().GetMethod(deltaMethodName);
                if (f == null) throw new NotImplementedException("Target of type " + target.GetType() +  " have'nt method, named " + deltaMethodName);
                target.GetType().GetMethod(deltaMethodName).Invoke(target, new object[] { Impact });
            }
            catch { throw; }
        }
    }
}