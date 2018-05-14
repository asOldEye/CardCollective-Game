using System;

namespace CardCollectiveSession
{
    /// <summary>
    /// Размещается у методов, способных быть вызванными из Modifier
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    sealed class Modified : Attribute { }

    /// <summary>
    /// Размещается у методов, способных быть вызванными из контроллера
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    sealed class ControllerCommand : Attribute
    {
        public bool OnMyTurn { get; }
        public ControllerCommand(bool onMyTurn = true) { OnMyTurn = onMyTurn; }
    }
}