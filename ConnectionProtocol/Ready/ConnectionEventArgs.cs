using System;

namespace ConnectionProtocol
{
    /// <summary>
    /// Событийные аргументы 
    /// </summary>
    public class ConnectionEventArgs : EventArgs
    {
        /// <summary>
        /// Возвращаемый объект
        /// </summary>
        public readonly object obj;

        internal ConnectionEventArgs(object obj)
        { this.obj = obj; }
    }
}