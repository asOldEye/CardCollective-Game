using System;
using AuxiliaryLibrary;

namespace CardCollectiveSession
{
    public class Destroyable : Component
    {
        public Destroyable(int healthMax, int health)
        {
            if ((HealthMax = healthMax) < 1)
                throw new ArgumentException("Wrong health max value, it must be more than zero");
            if ((Health = health) < 1 || Health > HealthMax)
                throw new ArgumentException("Wrong health max value, it must be more than zero and less than health max");
        }

        /// <summary>
        /// Максимальное здоровье карты
        /// </summary>
        public int HealthMax { get; private set; }

        private int health;
        /// <summary>
        /// Параметр здоровья карты
        /// </summary>
        public int Health
        {
            get { return health; }
            protected set
            {
                if (value < 0) value = 0;
                if (value > HealthMax) value = HealthMax;
                int delta = health - value;
                if (OnHealthChanged != null)
                    OnHealthChanged.Invoke(new SessionChange()); //TODO
                if ((health = value) == 0)
                    if (OnDeath != null) OnDeath.Invoke(new SessionChange()); //TODO
            }
        }

        /// <summary>
        /// Изменение здоровья объекта
        /// </summary>
        /// <param name="health">Количество изменяемых единиц здоровья</param>
        [Modified]
        public virtual void DeltaHealth(int delta)
        { Health += delta; }

        public override void Appear(Container container)
        {
            base.Appear(container);
            OnDeath += container.Session.SessionChanged;
            OnHealthChanged += container.Session.SessionChanged;
        }

        public event NonParametrizedEventHandler<SessionChange> OnDeath;
        public event NonParametrizedEventHandler<SessionChange> OnHealthChanged;
    }
}