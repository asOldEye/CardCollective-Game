using System;

namespace CardCollectiveSession
{
    /// <summary>
    /// Компонент контейнера
    /// </summary>
    public class Component
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public Container Container { get; set; }

        /// <summary>
        /// Вызывается перед началом хода игрока
        /// </summary>
        [Modified]
        public virtual void OnTurnEnds(Controller controller) { }
        /// <summary>
        /// Вызывается, когда сессия впервые переключает IsPlay в состояние true
        /// </summary>
        public virtual void OnSessionStart() { }

        /// <summary>
        /// Вызывается, когда контейнер появляется в сессии
        /// </summary>
        public virtual void OnContainerAppears(Container container)
        { Appear(container); }
        /// <summary>
        /// Вызывается, когда контейнер исчезает из сессии
        /// </summary>
        public virtual void OnContainerVanished(Container container)
        { Wanish(); }

        /// <summary>
        /// Вызывается при добавлении компонента к контейнеру
        /// </summary>
        public virtual void Appear(Container container)
        {
            if ((Container = container) == null)
                throw new ArgumentNullException(nameof(container));
            Container.OnAppears += OnContainerAppears;
            Container.OnVanished += OnContainerVanished;
            if (Container.Session != null)
            {
                Container.Session.OnSessionStart += OnSessionStart;
                Container.Session.OnTurnEnds += OnTurnEnds;
            }
        }
        /// <summary>
        /// Вызывается при удалении компонента из контейнера
        /// </summary>
        [Modified]
        public virtual void Wanish()
        {
            Container.OnAppears -= OnContainerAppears;
            Container.OnVanished -= OnContainerVanished;
            if (Container.Session != null)
            {
                Container.Session.OnSessionStart -= OnSessionStart;
                Container.Session.OnTurnEnds -= OnTurnEnds;
            }
            Container = null;
        }
    }
}