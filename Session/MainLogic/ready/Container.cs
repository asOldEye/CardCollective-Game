using System;
using System.Collections.Generic;
using AuxiliaryLibrary;

namespace CardCollectiveSession
{
    /// <summary>
    /// Контейнер для объектов
    /// </summary>
    public class Container
    {
        /// <summary>
        /// Идентификатор в сессии
        /// </summary>
        public int ID { get; private set; }
        /// <summary>
        /// Владелец этого объекта
        /// </summary>
        public Controller Owner { get; private set; }
        /// <summary>
        /// Установить владельца
        /// </summary>
        public virtual void SetOwner(Controller owner)
        {
            if (Owner != null) Owner.OwnedObjects.Remove(this);
            if ((Owner = owner) != null) Owner.OwnedObjects.Add(this);
        }
        /// <summary>
        /// Список модификаторов объекта
        /// </summary>
        public List<DurableModifier> Modifiers { get; } = new List<DurableModifier>();
        /// <summary>
        /// Сессия, в которой находится объект
        /// </summary>
        public Session Session
        { get; private set; }
        /// <summary>
        /// Добавить объект в сессию/убрать (s=null)
        /// </summary>
        public virtual void SetSession(Session s)
        {
            if (Session != null && (Session = s) == null) OnVanished.Invoke(this);
            else if (Session == null && (Session = s) != null)
            {
                ID = Session.AddObject(this);
                if (OnAppears != null)
                    OnAppears.Invoke(this);
            }
        }

        List<Component> components { get; } = new List<Component>();

        /// <summary>
        /// Возвращает компоненты
        /// </summary>
        public T GetComponent<T>() where T : Component
        {
            var comp = components.Find(new Predicate<Component>(f => f.GetType() == typeof(T)));
            if (comp != null) return comp as T;
            return null;
        }
        public Component GetComponent(Type T)
        {
            var comp = components.Find(new Predicate<Component>(f => f.GetType() == T));
            if (comp != null) return comp;
            return null;
        }
        /// <summary>
        /// Добавляет компонент
        /// </summary>
        public void AddComponent<T>(T component) where T : Component
        {
            if (component == null) throw new ArgumentNullException(nameof(component));
            if (GetComponent<T>() != null) throw new ArgumentException("Already contain this component");
            components.Add(component);
            component.Appear(this);
        }
        /// <summary>
        /// Удаляет компонент
        /// </summary>
        public bool DelComponent<T>(T component) where T : Component
        {
            if (GetComponent<T>() == null) return false;
            components.Remove(component);
            component.Wanish();
            return true;
        }

        public NonParametrizedEventHandler<Container> OnAppears;
        public NonParametrizedEventHandler<Container> OnVanished;
    }
}